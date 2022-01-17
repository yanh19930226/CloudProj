﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Entity.ViewModels
{
    /// <summary>
    ///     ck编辑器上传图片回调数据
    /// </summary>
    public class CKEditorUploadedResult
    {
        /// <summary>
        ///     1成功0失败
        /// </summary>
        public int uploaded { get; set; } = 0;

        /// <summary>
        ///     文件名称
        /// </summary>
        public string fileName { get; set; }

        /// <summary>
        ///     查看地址
        /// </summary>
        public string url { get; set; }

        /// <summary>
        ///     错误说明
        /// </summary>

        public CKEditorUploadedError error { get; set; }

        public object otherData { get; set; }
    }

    public class CKEditorUploadedError
    {
        public string message { get; set; }
    }
}