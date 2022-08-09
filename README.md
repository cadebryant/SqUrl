,;;:;,
;;;;;
,:;;:;    ,'=.
;:;:;' .=" ,'_\
':;:;,/  ,__:=@
';;:;  =./)_
 `"=\_  )_"`
         ``'"`

# Welcome to SqUrl - the Shortened Qualified URL generator!
SqUrl is a command-line tool for generating shortened URLs from normal ones.

In addition to auto-generating miniature URLs (6 characters), SqUrl also allows the user to create their own custom (vanity) URL - as long as it hasn't already been taken.

SqUrl provides the following functionalities:
1. Creating shortened URLs, both automatically and manually.
2. Deleting shortened URLs that have been previously added.
3. Retrieving the original URL via the shortened one.
4. Maintaining a count of original URL retrievals (per #3).

When the original URL is retrieved (i.e., by calling `Unshorten(..)`), the statistics for that URL are displayed onscreen.

**SqUrl is built on .NET 6.0 and requires the .NET 6.x runtime and Visual Studio 2022**.

Important: SqUrl runs in memory and does *not* persist data to a file or storage layer.  All data is cleared when SqUrl is shut down.

