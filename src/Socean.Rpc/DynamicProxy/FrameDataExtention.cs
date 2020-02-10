﻿using Socean.Rpc.DynamicProxy;

namespace Socean.Rpc.Core.Message
{
    public partial class FrameData
    {
        public string ReadContentString()
        {
            if (ContentBytes == null)
                return null;

            return DynamicProxySettings.DefaultEncoding.GetString(ContentBytes);
        }

        public string ReadTitleString()
        {
            if (TitleBytes == null)
                return null;

            return DynamicProxySettings.DefaultEncoding.GetString(TitleBytes);
        }
    }
}
