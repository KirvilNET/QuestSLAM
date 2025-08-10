using UnityEngine;

using RosSharp.RosBridgeClient;
using RosSharp;

using std_msgs = RosSharp.RosBridgeClient.MessageTypes.Std;
using geometry_msgs = RosSharp.RosBridgeClient.MessageTypes.Geometry;
using nav_msgs = RosSharp.RosBridgeClient.MessageTypes.Nav;
using sensor_msgs = RosSharp.RosBridgeClient.MessageTypes.Sensor;
using rosapi = RosSharp.RosBridgeClient.MessageTypes.Rosapi;

using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgcodecsModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.ObjdetectModule;
using OpenCVForUnity.UnityIntegration;

using PassthroughCameraSamples;
using UnityEngine.UI;
using System.Collections.Generic;
using OpenCVForUnity.UnityUtils;
using System;

namespace QuestSLAM.vision.VideoStreamer
{
    public class VideoStreamer : MonoBehaviour
    {
        Mat _webcamtextureMat;
        Mat _webcamtextureMat_RGBA;
        byte[] imageBytes;
        private sensor_msgs.Image image;

        void Stream(WebCamTextureManager _webcamtexture)
        {
            OpenCVMatUtils.WebCamTextureToMat(_webcamtexture.WebCamTexture, _webcamtextureMat);
            Imgproc.cvtColor(_webcamtextureMat, _webcamtextureMat_RGBA, Imgproc.COLOR_BGR2RGBA);
            OpenCVMatUtils.CopyFromMat(_webcamtextureMat_RGBA, imageBytes);


            image = new sensor_msgs.Image
            {
                header = new std_msgs.Header
                {
                    frame_id = "image",
                    stamp = new RosSharp.RosBridgeClient.MessageTypes.BuiltinInterfaces.Time
                    {
                        nanosec = (uint)UnityEngine.Time.time * 1000,
                        sec = (int)UnityEngine.Time.time
                    }
                },
                height = (uint)_webcamtextureMat_RGBA.height(),
                width = (uint)_webcamtextureMat_RGBA.width(),
                is_bigendian = 0,
                encoding = "rgba16",
                step = 16,
                data = imageBytes

            };
            
        }
    }
}