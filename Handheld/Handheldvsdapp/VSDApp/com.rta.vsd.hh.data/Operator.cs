using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSDApp.com.rta.vsd.hh.data
{
    class Operator
    {
        private string _operatorName;
        private string _operatorNameAr;
        private string _operatorTrafficFileNumber;        
        private Vehicle[] _topViolatingVehicles;
        private DateTime _companySuspensiondate;
        private string _operatorOVRRScore;
        private string _operatorODRRScore;
        private string _operatorOVRRColor;
        private string _operatorODRRColor;

        public string OperatorODRRColor
        {
            get { return _operatorODRRColor; }
            set { _operatorODRRColor = value; }
        }

        public string OperatorOVRRColor
        {
            get { return _operatorOVRRColor; }
            set { _operatorOVRRColor = value; }
        }

        public string OperatorODRRScore
        {
            get { return _operatorODRRScore; }
            set { _operatorODRRScore = value; }
        }



        public string OperatorName
        {
            get { return this._operatorName; }
            set { this._operatorName = value; }
        }
        public string OperatorNameAr
        {
            get { return this._operatorNameAr; }
            set { this._operatorNameAr = value; }
        }
        public DateTime CompanySuspensionDate
        {
            get { return this._companySuspensiondate; }
            set { this._companySuspensiondate = value; }
        }

        public string TrafficFileNumber
        {
            get { return this._operatorTrafficFileNumber; }
            set { this._operatorTrafficFileNumber = value; }
        }

        public string OperatorOVRRScore
        {
            get { return this._operatorOVRRScore; }
            set { this._operatorOVRRScore = value; }
        }

        public Vehicle[] TopViolatingVehicles
        {
            get { return this._topViolatingVehicles; }
            set { this._topViolatingVehicles = value; }
        }

    }
}
