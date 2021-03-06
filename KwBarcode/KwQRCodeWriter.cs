﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;
using com.google.zxing;
using com.google.zxing.common;
using com.google.zxing.qrcode;
using com.google.zxing.qrcode.decoder;
using EncodeHintType = com.google.zxing.EncodeHintType;
using ErrorCorrectionLevel = com.google.zxing.qrcode.decoder.ErrorCorrectionLevel;

namespace KwBarcode
{
    public enum QR_CORRECT_LEV
    {
        L = 0,
        M = 1,
        Q = 2,
        H = 3,
    };

    [Guid("4182EA72-DBD2-491B-A659-DC3F4A05707D")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IQRWriter
    {
        string FilePath { get; set; }
        byte[] RawByte { get; }
        string Text { get; }
        int Size { get; set; }
        void encodeAndSave(string text, string path);
        Bitmap textToQRImage(string text);
        void encodeAndSaveWithEC(string text, string path, QR_CORRECT_LEV correctLev);
    }

    [Guid("69F37639-F632-433F-AA01-1BC326FD1D6F")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("KwQRCodeWriter")]
    public class KwQRCodeWriter:IQRWriter
    {
        static private QRCodeWriter writer = null;
        private string filePath = "";
        private string text = "";
        byte[] rawByte = null;
        static private int size = 1024;

        public KwQRCodeWriter()
        {
            writer = new QRCodeWriter();
        }

        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        public int Size
        {
            get { return size; }
            set { size = value; }
        }

        public string Text
        {
            get { return text; }
        }

        public byte[] RawByte
        {
            get { return rawByte; }
        }

        public void encodeAndSave(string text, string path)
        {
            try
            {
                this.filePath = path;
                textToQRImage(text, QR_CORRECT_LEV.L).Save(path, ImageFormat.Bmp);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public Bitmap textToQRImage(string text)
        {
            return textToQRImage(text, QR_CORRECT_LEV.L);
        }

        public void encodeAndSaveWithEC(string text, string path, QR_CORRECT_LEV correctLev)
        {
            try
            {
                this.filePath = path;
                textToQRImage(text, (QR_CORRECT_LEV)correctLev).Save(path, ImageFormat.Bmp);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static public Bitmap textToQRImage2(string text)
        {
            return textToQRImage(text, QR_CORRECT_LEV.L);
        }

        static public Bitmap textToQRImage(string text, QR_CORRECT_LEV correctLev)
        {
            try
            {
                if (writer == null) writer = new QRCodeWriter();
                Hashtable hints = new Hashtable();
                switch (correctLev)
                {
                    case QR_CORRECT_LEV.L:
                        hints[EncodeHintType.ERROR_CORRECTION] = ErrorCorrectionLevel.L;
                        break;
                    case QR_CORRECT_LEV.M:
                        hints[EncodeHintType.ERROR_CORRECTION] = ErrorCorrectionLevel.M;
                        break;
                    case QR_CORRECT_LEV.Q:
                        hints[EncodeHintType.ERROR_CORRECTION] = ErrorCorrectionLevel.Q;
                        break;
                    case QR_CORRECT_LEV.H:
                        hints[EncodeHintType.ERROR_CORRECTION] = ErrorCorrectionLevel.H;
                        break;
                    default:
                        hints[EncodeHintType.ERROR_CORRECTION] = ErrorCorrectionLevel.L;
                        break;
                }
                ByteMatrix byteMatrix = writer.encode(
                        text,
                        BarcodeFormat.QR_CODE,
                        size,
                        size,
                        hints);

                return byteMatrix.ToBitmap();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

    }
}
