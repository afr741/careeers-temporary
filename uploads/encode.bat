for %%A IN (*.mp4) DO ffmpeg -i "%%A" "%%Aencoded.mp4"