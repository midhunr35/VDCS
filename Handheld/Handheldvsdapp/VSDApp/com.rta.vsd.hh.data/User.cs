using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSDApp.com.rta.vsd.hh.data
{
    class User
    {
        private string _FirstName;
        private string _lastName;
        private string _userName;
        private string _empPassword;
        private string _employeID;
        private string _desigination;
        private string _mobNumber;
        private string _FistNameAr;
        private string _LastNameAr;

        private string _desiginationAr;
        private byte[] _pictureString;
        private string _email;

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }


        private string _pictureFormat;

        public string EmpPassword
        {
            get { return _empPassword; }
            set { _empPassword = value; }
        }
        public string PictureFormat
        {
            get { return _pictureFormat; }
            set { _pictureFormat = value; }
        }

        public byte[] PictureString
        {
            get { return _pictureString; }
            set { _pictureString = value; }
        }

        public string DesiginationAr
        {
            get { return _desiginationAr; }
            set { _desiginationAr = value; }
        }



        public string LastNameAr
        {
            get { return _LastNameAr; }
            set { _LastNameAr = value; }
        }

        public string FistNameAr
        {
            get { return _FistNameAr; }
            set { _FistNameAr = value; }
        }

        public string MobNumber
        {
            get { return _mobNumber; }
            set { _mobNumber = value; }
        }

        public string Desigination
        {
            get { return _desigination; }
            set { _desigination = value; }
        }

        public string EmployeID
        {
            get { return _employeID; }
            set { _employeID = value; }
        }

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }

    }
}
