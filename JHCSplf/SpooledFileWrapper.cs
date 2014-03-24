using System;
using com.ibm.as400.access;

namespace JHCSplf
{
    public class SpooledFileWrapper
    {
        internal readonly SpooledFile file;

        public string File
        {
            get
            {
                return this.file.getName();
            }
        }

        public string User
        {
            get
            {
                return this.file.getJobUser();
            }
        }

        public string JobName
        {
            get
            {
                return this.file.getJobName();
            }
        }

        public DateTime Created
        {
            get
            {
                var CYYMMDDstring = this.file.getCreateDate().PadLeft(7, '0');
                var YYMMDDstring = CYYMMDDstring.Substring(1);
                var century = 19 + int.Parse(CYYMMDDstring.Substring(1, 1));
                var date = DateTime.ParseExact(century + YYMMDDstring, "yyyyMMdd", null);

                var time = this.file.getCreateTime();
                var timespan = TimeSpan.ParseExact(time, "hhmmss", null);


                return date.Add(timespan);


            }
        }

        public SpooledFileWrapper(SpooledFile file)
        {
            this.file = file;
        }


    }
}