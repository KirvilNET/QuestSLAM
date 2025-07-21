[![Build QuestSLAM App](https://github.com/KirvilNET/QuestSLAM/actions/workflows/build.yml/badge.svg?event=status)](https://github.com/KirvilNET/QuestSLAM/actions/workflows/build.yml)

# QuestSLAM

This project enables streaming of Quest3/3s tracking information over ROS Bridge for SLAM Mapping. This pose information can be used to localize a robot in 3 dimentional space while being light and cost effective. The sensors aboard the Quest3 and 3s have an accuracy of 

# Key Features
- Self-contained, rechargeable battery
- Network based data transmission (wired and wireless)
- Up to 120Hz tracking refresh rate
- Built on Unity allowing for easy modification for special use cases
- Powered by Qualcomm XR2G2 platform with multiple CPU/GPU cores, 8GB RAM, dedicated video hardware, and 128GB+ storage

# How it works
QuestSLAM is built on the Quest Visual-Inertial Odometry (VIO) system allowing it to track in 3d space with high levels of accuracy. 

- Capture data (IMU, VIO, Accelerometer)
- Process data and add corvaience
- Convert Unity coordinates to ROS cooordinates
- Generate ROS2 messages (Odometry, Battery%, Velocity)
- Send data over network (Ethernet or Wireless)

# Current Features

- Odometry messages over ROS bridge
- Basic headset telemetry (Batter precentage)
- Launch over ADB

# Features to come 

- Streaming of video from onboard cameras 
- Aprial tag recognition and assigning a transform to it 
- intergration into nav stack
-

# How to use 

- Download the latest version from Tags
- Install the APK (2 options)
    - Sidequest
    - ADB shell (adb install "path to apk")
- Launch the app over ADB
    - `adb shell am start -n com.kirvilnet.QuestSLAM/com.unity3d.player.UnityPlayerGameActivity -e "ip" "Bridge Server IP"`
    - port is 9090
- Enjoy


