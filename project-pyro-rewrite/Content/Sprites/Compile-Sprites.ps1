foreach($dir in Get-Content .\compile.txt) {
	if($dir -ne "") {
		.\SpriteAtlasPacker.exe -image:$dir/sheet.png -map:$dir/sheet.atlas $dir
	}
}