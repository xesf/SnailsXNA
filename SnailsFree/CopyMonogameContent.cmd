md ..\MonogameContent

rem MacOs
md ..\MonogameContent\MacOs
md ..\MonogameContent\MacOs\Content

xcopy Snails\bin\x86\Release\Content\*.* ..\MonogameContent\MacOs\Content /s /y
del ..\MonogameContent\MacOs\Content\Musics\* /Q
md ..\MonogameContent\MacOs\Content\Musics
xcopy SnailsResources\Musics\*.* ..\MonogameContent\MacOs\Content\Musics /s /y

rem Metro
md ..\MonogameContent\Metro
md ..\MonogameContent\Metro\Content

xcopy Snails\bin\x86\Release\Content\*.* ..\MonogameContent\Metro\Content /s /y

explorer ..\MonogameContent
