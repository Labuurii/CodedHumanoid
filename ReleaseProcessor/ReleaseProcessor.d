
import std.getopt;
import std.file;
import std.exception;
import std.path;
import std.stdio : writeln, File;
import std.format;

enum ProcessStage {
	PreProcess,
	PostProcess
}

void main(string[] args) {
	ProcessStage stage;

	{
		auto get_opt_res = getopt(args, config.passThrough,
			config.required, "stage", &stage);
			
		if(get_opt_res.helpWanted)
		{
			//TODO: Display info about all options. This is not adequate.
			defaultGetoptPrinter("ReleaseProcessor is here to automate the process of doing releases.", get_opt_res.options);
			return;
		}
	}
	
	chdir("..");
	
	with(ProcessStage) final switch(stage) {
		case PreProcess:
			pre_process(args);
			break;
		case PostProcess:
			post_process(args);
			break;
	}
}

struct Manifest {
public:
	int build_number;
	string semver_string;
}

void pre_process(string[] args) {
	immutable release_processor_path = "./ReleaseProcessor";
	immutable manifest_path = release_processor_path ~ "/manifest.txt";
	enum manifest_format = "last_release_number %s; last_release_semver %s";
	
	Manifest manifest;
	{
		auto manifest_string = readText!string(manifest_path);
		manifest_string.formattedRead!manifest_format
			(manifest.build_number, manifest.semver_string);
	}
	++manifest.build_number;
	writeln("New build manifest:");
	writeln(manifest);
	
	//TODO: Insert the manifest info into both the server and client builds
	
	{
		auto manifest_fd = File(manifest_path, "w");
		manifest_fd.lockingTextWriter
			.formattedWrite!manifest_format(manifest.build_number, manifest.semver_string);
	}
}

//TODO: Actual build step

void post_process(string[] args) {
	immutable zerobrane_name = "zerobrane";
	immutable zerobrane_windows_name = zerobrane_name ~ "_windows";
	
	immutable zero_brane_windows = zerobrane_windows_name;
	
	immutable release_dir = "release_bin";
	immutable release_dir_windows = release_dir ~ "/windows";
	
	if(!release_dir_windows.exists)
		release_dir_windows.mkdir;
	
	auto out_zero_brane_windows_path = buildPath(release_dir_windows, zerobrane_windows_name);
	copy_dir(zero_brane_windows, out_zero_brane_windows_path);
}

void copy_dir(string from, string to) {
	

	auto from_abs = from.absolutePath;
	auto to_abs = to.absolutePath;
	if(to.exists)
		to.rmdirRecurse();
	
	foreach(entry; dirEntries(from, SpanMode.depth, false)) {
		auto relative_path = entry.name.absolutePath.relativePath(from_abs);
		auto to_path = relative_path.absolutePath(to_abs);
		writeln(relative_path);
		if(entry.isFile) {
			{
				auto dir_rel_path = relative_path.dirName;
				mkdirRecurse(dir_rel_path.absolutePath(to_abs));
			}
		
			copy(entry.name, to_path);
		} else {
			mkdirRecurse(to_path);
		}
	}
}