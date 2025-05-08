#!/bin/bash
cd ..
gource --key -f -r 30 --disable-progress --seconds-per-day 3 --auto-skip-seconds 0.1 -1920x1080 -o - | ./ffmpeg -hwaccel nvdec -y -f image2pipe -vcodec ppm -i - -c:v hevc_nvenc gource.mp4