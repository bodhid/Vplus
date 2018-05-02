# V+ Video Player
Plays any video at the refresh rate of your monitor

-Uses ViveMediaDecoder and StandaloneFileBrowser


Known issues  
-Frame-Interpolation: When a video contains only color (Fade to Black), the output image may flicker  
-Frame-Interpolation: When playing a large file, seeking may cause brief flickering  
-Frame-Interpolation: doesn't like Cartoons/Animes  
-Frame-Interpolation: Artifacts are visible when playing a video with the same framerate as the display monitor (kinda defeats the purpose)  
-Escape does not exit fullscreen mode  
-Seeking with hotkeys can get stuck on the same position when the video's keyframes are too far apart  
-Yes it says "Press H for help" but you need to hold the button, or read really fast  

Changelog  
0.1804.09  
-Resyncing Frame-Interpolation is more subtle to avoid skipping frames and hickups  
-FPS and Interpolation Debugger added to monitor standalone build, CTRL+D  

0.1804.06  
-Frame-Interpolation can now handle variable framerates  

0.1804.05  
-Frame-Interpolation works with all videos (web or local)  
-Frame calculation added for videos with no valid framerate metadata  
-Gamepad support added (XInput)  

0.1804.04  
-GPU Accelerated Frame-Interpolation added  
-Output color range switched from limited to full  

0.1804.03  
-UI scales to the monitor's display scale setting  
-Custom UI scaling added with CTRL + Scrollwheel  

0.1803.31  
-Automatically scans the video's folder for subtitle files  


Video:

[![IMAGE ALT TEXT](http://img.youtube.com/vi/iP809ebKFgs/0.jpg)](http://www.youtube.com/watch?v=iP809ebKFgs "Video Title")
