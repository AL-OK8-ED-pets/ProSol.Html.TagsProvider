rm -rf .nuget
dotnet pack -c Release -p:NuspecFile=package.nuspec --include-source -o=.nuget
