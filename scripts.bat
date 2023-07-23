rm scripts.zip
copy .\client\*.cs.dso .\scripts\client\materials.cs.dso
"C:\Program Files\7-Zip\7z.exe" a scripts.zip -pchangeme *.cs.dso *.png LICENSE -r