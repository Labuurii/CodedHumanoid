cd "../CloudBuildDownloader/CloudBuildDownloader/bin/Debug"
CloudBuildDownloader --build_mode=Debug --build_name="win64-debug" --platform="Windows" --project_name="pl-war"
cd ../../../../ReleaseProcessor
dmd -debug -g -run ReleaseProcessor.d --stage=PostProcess