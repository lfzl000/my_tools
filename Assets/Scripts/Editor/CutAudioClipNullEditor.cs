using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public static class CutAudioClipNullEditor
{
    private static List<AudioClip> curSelectAudioClip;
    private static List<string> curSelectAudioClipPath;

    [MenuItem(@"Tools/AudioClip Cut Null")]
    public static void GetAllAudioClip()
    {
        object[] objs = Selection.objects;
        int objsCount = objs.Length;
        curSelectAudioClip = new List<AudioClip>();
        curSelectAudioClipPath = new List<string>();
        for (int i = 0; i < objsCount; i++)
        {
            AudioClip curClip = objs[i] as AudioClip;
            if (curClip != null)
            {
                string curPath = AssetDatabase.GetAssetPath(curClip);
                string[] curPathArr = curPath.Split('/');
                string curFoldPath = "";
                for (int j = 0; j < curPathArr.Length; j++)
                {
                    if (j != curPathArr.Length - 1)
                    {
                        curFoldPath += curPathArr[j] + "/";
                    }
                }
                string[] curfileName = curPath.Split('.');
                string format = "." + curfileName[curfileName.Length - 1];
                curSelectAudioClip.Add(curClip);
                curSelectAudioClipPath.Add(curPath);
                //Debug.Log(curClip.name + " _ PATH _ : " + curPath);

                CutNull(curClip, curFoldPath, format);

                Debug.Log(">>>>>>>> CUT NULL AUDIO CLIP : " + curClip.name);
            }
            bool isCancle = EditorUtility.DisplayCancelableProgressBar("切除音频空白", "正在切除音频空白内容..." + i + "/" + objsCount, (float)i / objsCount);
            if (isCancle || i >= objsCount - 1)
            {
                EditorUtility.ClearProgressBar();
            }
        }

        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 切除空白
    /// </summary>
    /// <param name="clip">音频</param>
    /// <param name="_foldPath">文件夹路径</param>
    /// <param name="_format">音频格式</param>
    private static void CutNull(AudioClip clip, string _foldPath, string _format)
    {
        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        List<float> newSamples = new List<float>();

        newSamples = CutArrayNull(samples, true);

        newSamples.Reverse();

        newSamples = CutArrayNull(newSamples.ToArray(), false);

        newSamples.Reverse();

        string newName = "cut_null_" + clip.name;

        AudioClip new_Clip = AudioClip.Create(newName, newSamples.Count, clip.channels, clip.frequency, false);
        new_Clip.SetData(newSamples.ToArray(), 0);

        Save(new_Clip, _foldPath + newName + _format);
    }

    public static float positiveMinValue = 0.01f;
    public static float negativeMinValue = 0.01f;
    private static List<float> CutArrayNull(float[] data, bool _isPositive)
    {
        float minValue = 0;
        minValue = _isPositive ? positiveMinValue : negativeMinValue;

        List<float> newSamples = new List<float>();

        int c = data.Length;
        int s = 0;
        for (int i = 0; i < c; i++)
        {
            if (s == 0)
            {
                if (Mathf.Abs(data[i]) >= minValue)
                {
                    s = i;
                }
            }
            else
            {
                if (i >= s)
                {
                    newSamples.Add(data[i]);
                }
            }
        }

        return newSamples;
    }


    public static void Save(AudioClip clip, string path)
    {
        string filePath = Path.GetDirectoryName(path);
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        using (FileStream fileStream = CreateEmpty(path))
        {
            ConvertAndWrite(fileStream, clip);
            WriteHeader(fileStream, clip);
        }
    }

    private static void ConvertAndWrite(FileStream fileStream, AudioClip clip)
    {

        float[] samples = new float[clip.samples];

        clip.GetData(samples, 0);

        Int16[] intData = new Int16[samples.Length];

        Byte[] bytesData = new Byte[samples.Length * 2];

        int rescaleFactor = 32767; //to convert float to Int16  

        for (int i = 0; i < samples.Length; i++)
        {
            intData[i] = (short)(samples[i] * rescaleFactor);
            Byte[] byteArr = new Byte[2];
            byteArr = BitConverter.GetBytes(intData[i]);
            byteArr.CopyTo(bytesData, i * 2);
        }
        fileStream.Write(bytesData, 0, bytesData.Length);
    }

    private static FileStream CreateEmpty(string filepath)
    {
        FileStream fileStream = new FileStream(filepath, FileMode.Create);
        byte emptyByte = new byte();

        for (int i = 0; i < 44; i++) //preparing the header  
        {
            fileStream.WriteByte(emptyByte);
        }

        return fileStream;
    }

    private static void WriteHeader(FileStream stream, AudioClip clip)
    {
        int hz = clip.frequency;
        int channels = clip.channels;
        int samples = clip.samples;

        stream.Seek(0, SeekOrigin.Begin);

        Byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
        stream.Write(riff, 0, 4);

        Byte[] chunkSize = BitConverter.GetBytes(stream.Length - 8);
        stream.Write(chunkSize, 0, 4);

        Byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
        stream.Write(wave, 0, 4);

        Byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
        stream.Write(fmt, 0, 4);

        Byte[] subChunk1 = BitConverter.GetBytes(16);
        stream.Write(subChunk1, 0, 4);

        UInt16 two = 2;
        UInt16 one = 1;

        Byte[] audioFormat = BitConverter.GetBytes(one);
        stream.Write(audioFormat, 0, 2);

        Byte[] numChannels = BitConverter.GetBytes(channels);
        stream.Write(numChannels, 0, 2);

        Byte[] sampleRate = BitConverter.GetBytes(hz);
        stream.Write(sampleRate, 0, 4);

        Byte[] byteRate = BitConverter.GetBytes(hz * channels * 2); // sampleRate * bytesPerSample*number of channels, here 44100*2*2  
        stream.Write(byteRate, 0, 4);

        UInt16 blockAlign = (ushort)(channels * 2);
        stream.Write(BitConverter.GetBytes(blockAlign), 0, 2);

        UInt16 bps = 16;
        Byte[] bitsPerSample = BitConverter.GetBytes(bps);
        stream.Write(bitsPerSample, 0, 2);

        Byte[] datastring = System.Text.Encoding.UTF8.GetBytes("data");
        stream.Write(datastring, 0, 4);

        Byte[] subChunk2 = BitConverter.GetBytes(samples * channels * 2);
        stream.Write(subChunk2, 0, 4);
    }
}