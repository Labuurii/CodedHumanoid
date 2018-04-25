cd ..
cd "CloudBuildDownloader/CloudBuildDownloader/bin/Debug"
CloudBuildDownloader --build_mode=Release --build_name="win64-release" --platform="Windows" --project_name="pl-war"
cd ../../../../ReleaseProcessor
dmd -debug -g -run ReleaseProcessor.d --stage=PostProcess --build_mode=Release