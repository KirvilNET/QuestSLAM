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
using OpenCVForUnity.Calib3dModule;

using PassthroughCameraSamples;
using UnityEngine.UI;
using System.Collections.Generic;
using OpenCVForUnity.UtilsModule;


namespace QuestSLAM.vision

{
    public class TagManagaer : MonoBehaviour
    {
        [SerializeField] WebCamTextureManager camTextureManager;
        [SerializeField] RawImage preview;

        public Mat _webcamtexture;
        public Mat _webcamtexture_grey;
        public Mat _webcamtexture_blur;

        List<Mat> DetectedMarkerPos;
        List<Mat> rejectDetectedMarkerPos;
        Size boardSize = new Size(9, 6); // Number of inner corners (columns, rows)
        float squareLength = 0.023f;
        private List<Mat> objectPoints = new List<Mat>();
        private List<Mat> imagePoints = new List<Mat>();
        private List<Point3> points;
        private List<Mat> rvecs; // Rotation vectors
        private List<Mat> tvecs; // Translation vectors  
        Mat camMatrix;
        Mat pointsMAT;
        MatOfDouble distCoeffs;
        private Mat DetectedMarkerID;
        public Texture2D previewtexture;
        ArucoDetector arucoDetector;
        public Mat _previewTexture;
        Mat undistort;

        Mat Rot;
        Mat Pos;

        private MatOfPoint3f _markerObjectPoints;

        Vector3 ConvertTvecToUnityVector3(Mat tvec)
        {
            double x = tvec.get(0, 0)[0]; 
            double y = tvec.get(1, 0)[0];
            double z = tvec.get(2, 0)[0];
            
            return new Vector3((float)x, (float)-y, (float)z); 
        }

        Quaternion ConvertRvecToUnityQuaternion(Mat rvec)
        {
            double x = rvec.get(0, 0)[0]; 
            double y = rvec.get(1, 0)[0];
            double z = rvec.get(2, 0)[0];

            float angle = (float)(Mathf.Sqrt((float)(x * x + y * y + z * z)) * 180 / Mathf.PI);
            
            Vector3 axis = new Vector3((float)-x, (float)y, (float)-z);

            return Quaternion.AngleAxis(angle, axis);
        }

        void showResult()
        {
            Objdetect.drawDetectedMarkers(_previewTexture, DetectedMarkerPos, DetectedMarkerID, new Scalar(0, 255, 0));
            OpenCVMatUtils.MatToTexture2D(_previewTexture, previewtexture);
            preview.texture = previewtexture;
            
        }

        public void calibrateCamera()
        {

            MatOfPoint2f corners = new MatOfPoint2f();
            bool found = Calib3d.findChessboardCorners(_webcamtexture_grey, boardSize, corners);

            if (found)
            {
                TermCriteria criteria = new TermCriteria(TermCriteria.EPS + TermCriteria.MAX_ITER, 30, 0.001);
                Calib3d.find4QuadCornerSubpix(_webcamtexture_grey, corners, new Size(11, 11));

                // Create 3D object points for the chessboard pattern
                MatOfPoint3f obj = new MatOfPoint3f();
                List<Point3> objPointsList = new List<Point3>();
                for (int i = 0; i < boardSize.height; i++)
                {
                    for (int j = 0; j < boardSize.width; j++)
                    {
                        objPointsList.Add(new Point3(j * squareLength, i * squareLength, 0));
                    }
                }
                obj.fromList(objPointsList);

                // Store the detected 2D image points and their corresponding 3D object points
                imagePoints.Add(corners);
                objectPoints.Add(obj);
            }
            else
            {
                Debug.LogWarning("Chessboard corners not found in the image.");
            }

            Calib3d.calibrateCamera(objectPoints, imagePoints, _webcamtexture_grey.size(), camMatrix, distCoeffs, rvecs, tvecs);

            PlayerPrefs.SetString("CameraMatrix", camMatrix.ToString());
            PlayerPrefs.SetString("distCoffs", distCoeffs.ToString());

            PlayerPrefs.Save();

        }

        public void detectAprialTag()
        {

            _webcamtexture = new Mat(camTextureManager.WebCamTexture.height, camTextureManager.WebCamTexture.width, CvType.CV_8UC4);
            _webcamtexture_grey = new Mat(camTextureManager.WebCamTexture.height, camTextureManager.WebCamTexture.width, CvType.CV_8UC4);
            _webcamtexture_blur = new Mat(camTextureManager.WebCamTexture.height, camTextureManager.WebCamTexture.width, CvType.CV_8UC4);
            undistort = new Mat(camTextureManager.WebCamTexture.height, camTextureManager.WebCamTexture.width, CvType.CV_8UC4);


            _previewTexture = new Mat(camTextureManager.WebCamTexture.height, camTextureManager.WebCamTexture.width, CvType.CV_8UC4);

            DetectedMarkerID = new Mat();
            DetectedMarkerPos = new List<Mat>();
            rejectDetectedMarkerPos = new List<Mat>();

            Rot = new Mat();
            Pos = new Mat();



            if (previewtexture == null)
            {
                previewtexture = new Texture2D(_previewTexture.cols(), _previewTexture.rows());
            }

            OpenCVMatUtils.WebCamTextureToMat(camTextureManager.WebCamTexture, _webcamtexture);
            Imgproc.cvtColor(_webcamtexture, _webcamtexture_grey, 7);
            Imgproc.GaussianBlur(_webcamtexture_grey, _webcamtexture_blur, new Size(3, 3), 0);
            //Calib3d.undistort(_webcamtexture_blur, undistort, camMatrix, distCoeffs);


            arucoDetector.detectMarkers(_webcamtexture_blur, DetectedMarkerPos, DetectedMarkerID, rejectDetectedMarkerPos);

            if (DetectedMarkerID.total() > 0)
            {
                for (int i = 0; i < DetectedMarkerID.total(); i++)
                {
                    int markerId = (int)DetectedMarkerID.get(i, 0)[0];
                    Mat currentCornersMat = DetectedMarkerPos[i];

                    MatOfPoint2f imagePoints = new MatOfPoint2f();
                    List<Point> cornersList = new List<Point>();



                    for (int j = 0; j < currentCornersMat.cols(); j++)
                    {
                        double[] p = currentCornersMat.get(0, j); // Get point (x,y)
                        cornersList.Add(new Point(p[0], p[1])); // Extract individual coordinates from the array
                    }
                    imagePoints.fromList(cornersList);

                    double[] p0 = currentCornersMat.get(0, 0); // Top-left corner
                    Point topLeft = new Point(p0[0], p0[1]);

                    double[] p1 = currentCornersMat.get(0, 1); // Top-right corner
                    Point topRight = new Point(p1[0], p1[1]);

                    double[] p2 = currentCornersMat.get(0, 2); // Bottom-right corner
                    Point bottomRight = new Point(p2[0], p2[1]);

                    double[] p3 = currentCornersMat.get(0, 3); // Bottom-left corner
                    Point bottomLeft = new Point(p3[0], p3[1]);

                    Calib3d.solvePnP((MatOfPoint3f)pointsMAT, imagePoints, camMatrix, distCoeffs, Rot, Pos);

                    Vector3 position = ConvertTvecToUnityVector3(Pos);
                    Quaternion rotation = ConvertRvecToUnityQuaternion(Rot);

                    Debug.Log($"Detected AprilTag ID: {markerId}: position: {position}, Rotation: {rotation}");

                    imagePoints.Dispose();
                    currentCornersMat.Dispose();
                }
            }

            showResult();

            foreach (var cornerMat in DetectedMarkerPos)
            {
                cornerMat.Dispose();
            }
            foreach (var rejectcornerMat in rejectDetectedMarkerPos)
            {
                rejectcornerMat.Dispose();
            }
            DetectedMarkerPos.Clear();
            DetectedMarkerID.Dispose();

            _webcamtexture_blur.Dispose();
            _webcamtexture.Dispose();
            _webcamtexture_grey.Dispose();
            _previewTexture.Dispose();
            undistort.Dispose();
            Rot.Dispose();
            Pos.Dispose();
        }

        bool cameraCalServiceHandler(rosapi.GetParamRequest request, out rosapi.GetParamResponse response)
        {
            Debug.Log("recived request: " + request.name);

            calibrateCamera();
            response = new rosapi.GetParamResponse
            {
                value = "Calibrating Camera"

            };
            return true;
        }



        public void genService(RosSocket socket)
        {
            socket.AdvertiseService<rosapi.GetParamRequest, rosapi.GetParamResponse>("/QuestSLAM/camera/calibrate", cameraCalServiceHandler);
        }



        void Start()
        {
            Dictionary dictionary = Objdetect.getPredefinedDictionary(20);
            DetectorParameters detectorParams = new DetectorParameters();
            detectorParams.set_cornerRefinementMethod(Objdetect.CORNER_REFINE_APRILTAG);
            detectorParams.set_aprilTagMinClusterPixels(200);

            RefineParameters refineParameters = new RefineParameters(10f, 3f, true);

            arucoDetector = new ArucoDetector(dictionary, detectorParams, refineParameters);

            previewtexture = new Texture2D(_previewTexture.width(), _previewTexture.height(), TextureFormat.RGBA32, true);


            double fx = (10 * _webcamtexture.width()) / 6;
            double fy = (10 * _webcamtexture.height()) / 6;
            double cx = _webcamtexture.width() / 2.0f;
            double cy = _webcamtexture.height() / 2.0f;
            camMatrix = new Mat(3, 3, CvType.CV_64FC1);
            camMatrix.put(0, 0, fx);
            camMatrix.put(0, 1, 0);
            camMatrix.put(0, 2, cx);
            camMatrix.put(1, 0, 0);
            camMatrix.put(1, 1, fy);
            camMatrix.put(1, 2, cy);
            camMatrix.put(2, 0, 0);
            camMatrix.put(2, 1, 0);
            camMatrix.put(2, 2, 1.0f);
            //Debug.Log("camMatrix " + camMatrix.dump());

            distCoeffs = new MatOfDouble(0, 0, 0, 0);
            double AcroSize = 0.200;
            
            points.Add(new Point3(-AcroSize/ 2, AcroSize / 2, 0));
            points.Add(new Point3(AcroSize/ 2, AcroSize / 2, 0));
            points.Add(new Point3(AcroSize/ 2, -AcroSize / 2, 0));
            points.Add(new Point3(-AcroSize/ 2, -AcroSize / 2, 0));

            
            pointsMAT = Converters.vector_Point3f_to_Mat(points);
        }
        

    }
}

