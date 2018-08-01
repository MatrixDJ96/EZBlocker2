# EZBlocker 2

### This is an <i>unofficial</i> successor of [EZBlocker][1] by Eric Zhang

#

### Information

EZBlocker 2 is ad muter/blocker for Spotify.<br>
It is compatible with every O.S. which supports <b>.NET Framework 4.5+</b>.<br>
(<b>Also Windows Store version is supported!</b>)

You could choose 2 different options:
- Mute ads (if an ad does load, it will mute Spotify until the ad is over)
- Block ads (apply hosts patches on system to prevent ads on Spotify from loading)

<b>No setup required, it's a portable application!</b><br>
To update your EZBlocker 2, replace the executable or use <b>auto-update</b> process.<br>

#

### How it works

Mute ads:
- <s>Basically Spotify desktop runs a web server on 127.0.0.1 that contains information about Spotify status. EZBlocker 2 attempts to extract song information hooking to this local server. To use this non-intrusive method, web helper in Spotify must be enabled.</s>
- EZBlocker 2 attempts to extract song information hooking to the new Spotify Web API. To use this official method you need to connect with your Spotify account (using any browser) and it is necessary to accept the request. (<a href="https://developer.spotify.com/assets/oauth.png">Example</a>)

Block ads:
- To prevent ads on Spotify from loading EZBlocker 2 must apply <b>hosts patches</b> on your system. These patches to be applied should require administrator rights.
- Enabling this option will be disabled automatic update for Spotify!

### Error reporting

EZBlocker 2 will create a log with all errors detected. If you want to open a issue on GitHub, it would be better to attach that log to receive a better solution.

#

### [Download for Windows][7]

#

### Credits

- MatrixDJ96 (me) for this amazing project
- Eric Zhang for the original [EZBlocker][1]
- [Bruske][2] for design inspiration
- <s>Shyyko Serhiy for [spotilocal][3] method</s>
- FadeMind for [hosts][4] patches
- Mark Heath for [NAudio][5] library
- James Newton-King for [Json.NET][6] library

#

### Changelog

Version 2.1.3.1:
- Improved error handling
- Improved and fixed WebServer
- Decreased (half) request for second
- Improved error reporting (log)
- Little fixes

Version 2.1.3.0:
- Using new Spotify WebAPI (beta)

Version 2.1.2.5:
- Implemented save location on the screen
- Added CTRL key to center location on the screen
- Fixed commit 8d43ec8
- Updated hosts patches
- Bug fixes and general optimization

Version 2.1.2.4:
-  Fixed memory leak (issue #10, #9)

Version 2.1.2.3:
- Improved Spotify port detection
- Fixed minor bugs
- Added Spotilocal timeout
- Updated hosts patches
- Added warning when enabling hosts patches
- Fixed issue #6 (1st second or half second of ads is not muted)

Version 2.1.2.2:
- Updated hosts patches
- Fixed some weird title/artist/album song

Version 2.1.2.1:
- Improved update form
- Added tooltip for errors
- Added detection of .NET Framework 4.5
- Removed useless using directive
- Fixed version number

Version 2.1.2.0:
- Added info message when you enable/disable hosts patches
- Revert 'Downgraded .NET Framework to v4.0'
- Updated JSON library
- Improved update process
- Improved SpotilocalStatus
- Removed useless using directives
- Fixed minor bug

Version 2.1.1.0:
- Improved and fixed Spotify status checking process
- Updated user agent

Version 2.1.0.0:
- Significant improvement of the Spotify status checking process
- Updated hosts patches
- Fixed minor bugs

Version 2.0.0.8:
- Fixed minor crashes
- Fixed update process
- Fixed host patches applying process
- Fixed hosts patches
- Options StartMinized and StartOnLogin separated
- Added all security protocols for the network connect
- Changed exit timeout to 3 seconds
- Fixed issue #1 (the last second of ads are not muted)

Version 2.0.0.7:
- Added SndVol32.exe check for system volume mixer
- Improved songs detection
- Updated 7-Zip binary
- Improved errors reporting
- Updated host patches
- Improved host patches applying process
- Fixed minor bugs

Version 2.0.0.6:
- Downgraded .NET Framework to v4.0
- Improved Spotify detection process
- Improved processes starting mechanism

Version 2.0.0.5:
- Improved block-ads process
- Improved loading process
- Updated icons

Version 2.0.0.4:
- Fixed detection of Spotify in a particular case
- New settings storage (portable)
- Fixed typo in a message
- Fixed crash while playing local song
- Fixed detection of Spotify in multi-user case
- Added message when something goes wrong while extracting dependencies
- Added message and solution when the update process goes wrong
- Updated icon

Version 2.0.0.3:
- Updated graphic resources
- Fixed file extraction
- Recompiled NAudio.dll for .NET Framework 4.5
- Fixed "Loading EZBlocker 2..." message
- Added detection of .NET Framework 4.5
- Fixed start minimized option
- Fixed behavior when something goes wrong
- Updated websites
- Added GUI for update process
- Added more movement points

Version 2.0.0.2:
- Fixed auto-update process
- Fixed behavior if admin rights not granted

Version 2.0.0.1:
- Mute ads is now enabled by default

Version 2.0.0.0:
- EZBlocker 2! (Project rewritten from scratch)

[1]: https://github.com/Xeroday/Spotify-Ad-Blocker
[2]: https://github.com/Bruske
[3]: https://github.com/ShyykoSerhiy/spotilocal
[4]: https://github.com/FadeMind/hosts.extras
[5]: https://github.com/naudio/NAudio
[6]: https://github.com/JamesNK/Newtonsoft.Json
[7]: https://github.com/MatrixDJ96/EZBlocker2/releases
