
import std.getopt;
import std.file;
import std.exception;
import std.path;
import std.stdio : writeln, File;
import std.format;
import std.regex;
import std.conv : to;

enum ProcessStage {
	PreProcess,
	PostProcess
}

enum BuildMode {
	Invalid,
	Debug,
	Release
}

void main(string[] args) {
	ProcessStage stage;
	BuildMode build_mode;

	{
		auto get_opt_res = getopt(args, config.passThrough,
			config.required, "stage", &stage,
			"build_mode", &build_mode);
			
		if(get_opt_res.helpWanted)
		{
			defaultGetoptPrinter("ReleaseProcessor is here to automate the process of doing releases.", get_opt_res.options);
			return;
		}
	}
	
	chdir("..");
	
	with(ProcessStage) final switch(stage) {
		case PreProcess:
			pre_process();
			break;
		case PostProcess:
			post_process(build_mode);
			break;
	}
}

struct Manifest {
public:
	int build_number;
	string semver_string;
}

void pre_process() {
	immutable release_processor_path = "./ReleaseProcessor";
	immutable manifest_path = release_processor_path ~ "/manifest.txt";
	enum manifest_format = "last_release_number %s; last_release_semver %s";
	
	immutable client_scripts_path = "Assets/Game/Scripts";
	immutable client_version_data_script_path = client_scripts_path ~ "/GameVersionData.cs";
	
	immutable main_server_v2_version_data_path = "MainServerV2/Config.cs";
	
	Manifest manifest;
	{
		auto manifest_string = readText!string(manifest_path);
		manifest_string.formattedRead!manifest_format
			(manifest.build_number, manifest.semver_string);
	}
	++manifest.build_number;
	writeln("New build manifest:");
	writeln(manifest);
	writeln();
	
	writeln("New client version data:");
	change_version_data_of_file(client_version_data_script_path,
		regex("SemVerVersion *= *\"[0-9.,]*\""),
		regex("BuildNumber *= *[0-9]+"),
		"SemVerVersion = \"" ~ manifest.semver_string ~ "\"",
		"BuildNumber = " ~ manifest.build_number.to!string
	);
	writeln();
	writeln("New server version data:");
	change_version_data_of_file(main_server_v2_version_data_path,
		regex("GameVersion *= *\"[0-9.,]+\""),
		regex("GameBuildNumber *= *[0-9]+"),
		"GameVersion = \"" ~ manifest.semver_string ~ "\"",
		"GameBuildNumber = " ~ manifest.build_number.to!string
	);
	writeln();
	
	{
		auto manifest_fd = File(manifest_path, "w");
		manifest_fd.lockingTextWriter
			.formattedWrite!manifest_format(manifest.build_number, manifest.semver_string);
	}
}

void change_version_data_of_file(string file, 
	Regex!char semver_regex, 
	Regex!char build_num_regex, 
	string semver_replacement, 
	string build_num_replacement)
{
	auto client_version_text = readText!string(file);
	auto new_client_version_text = replaceFirst(client_version_text, semver_regex, semver_replacement);
	new_client_version_text = replaceFirst(new_client_version_text, build_num_regex, build_num_replacement);
	
	writeln(new_client_version_text);
	write(file, new_client_version_text);
}

void post_process(BuildMode build_mode) {
	immutable zerobrane_name = "zerobrane";
	immutable zerobrane_windows_name = zerobrane_name ~ "_windows";
	
	immutable zero_brane_windows = zerobrane_windows_name;
	
	immutable release_dir = "release_bin";
	immutable release_dir_windows = release_dir ~ "/windows";
	
	immutable debug_dir = "debug_bin";
	immutable debug_dir_windows = debug_dir ~ "/windows";
	
	string bin_dir;
	with(BuildMode) final switch(build_mode) {
		case Invalid:
			throw new Exception("Build mode is not specified");
		case Debug:
			bin_dir = debug_dir_windows; break;
		case Release:
			bin_dir = release_dir_windows; break;
	}
	
	if(!bin_dir.exists)
		bin_dir.mkdir;
	
	auto out_zero_brane_windows_path = buildPath(bin_dir, zerobrane_windows_name);
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