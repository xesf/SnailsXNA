using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace FontEdit
{
    class Helper
    {
        /// <summary>
        /// 
        /// </summary>
        public static byte[] StructToByteArray(object structure)
        {
            byte[] buffer = new byte[Marshal.SizeOf(structure)];
            GCHandle h = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            Marshal.StructureToPtr(structure, h.AddrOfPinnedObject(), false);
            h.Free();
            return buffer;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ReadStrucureFromStream(BinaryReader br, ref object structure)
        {
            byte[] buffer;
            buffer = new byte[Marshal.SizeOf(structure)];
            br.Read(buffer, 0, buffer.Length);
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            structure = Marshal.PtrToStructure(handle.AddrOfPinnedObject(), structure.GetType());
            handle.Free();
        }

        /// <summary>
        /// 
        /// </summary>
        public static void WriteStructureToFile(BinaryWriter br, object structure)
        {
            byte[] buffer = Helper.StructToByteArray(structure);
            br.Write(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        public static object ReadStructureFromFile(BinaryReader br, Type structType)
        {
            IntPtr ptr = IntPtr.Zero;
            try
            {
                byte[] buffer = new byte[Marshal.SizeOf(structType)];
                br.Read(buffer, 0, buffer.Length);
                ptr = Marshal.AllocHGlobal(buffer.Length);
                Marshal.Copy(buffer, 0, ptr, buffer.Length);

                return Marshal.PtrToStructure(ptr, structType);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string RelativePath(string absolutePath, string relativeTo)
        {
            if (relativeTo == null)
                return relativeTo;

            if (relativeTo[0] == '.' || relativeTo[0] == '\\')
                return relativeTo;

            string[] absoluteDirectories = absolutePath.Split('\\');
            string[] relativeDirectories = relativeTo.Split('\\');

            //Get the shortest of the two paths
            int length = absoluteDirectories.Length < relativeDirectories.Length ? absoluteDirectories.Length : relativeDirectories.Length;

            //Use to determine where in the loop we exited
            int lastCommonRoot = -1;
            int index;

            //Find common root
            for (index = 0; index < length; index++)
            {
                if (absoluteDirectories[index] == relativeDirectories[index])
                {
                    lastCommonRoot = index;
                }
                else
                    break;
            }

            //If we didn't find a common prefix then throw
            if (lastCommonRoot == -1)
            {
                throw new ArgumentException("Paths do not have a common base");
            }

            //Build up the relative path
            StringBuilder relativePath = new StringBuilder();

            //Add on the ..
            for (index = lastCommonRoot + 1; index < absoluteDirectories.Length; index++)
            {
                if (absoluteDirectories[index].Length > 0)
                    relativePath.Append("..\\");
            }

            //Add on the folders
            for (index = lastCommonRoot + 1; index < relativeDirectories.Length - 1; index++)
            {
                relativePath.Append(relativeDirectories[index] + "\\");
            }
            relativePath.Append(relativeDirectories[relativeDirectories.Length - 1]);

            return relativePath.ToString();
        }
    }
}
