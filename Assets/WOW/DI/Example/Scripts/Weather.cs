using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WOW.DI.Example
{
    public class Weather
    {
        public string PublishingOffice { get => this.publishingOffice; }
        public string ReportDateTime { get => this.reportDateTime; }
        public string TargetArea { get => this.targetArea; }
        public string HeadlineText { get => this.headlineText; }
        public string Text { get => this.text; }

        [SerializeField]
        private string publishingOffice = string.Empty;
        [SerializeField]
        private string reportDateTime = string.Empty;
        [SerializeField]
        private string targetArea = string.Empty;
        [SerializeField]
        private string headlineText = string.Empty;
        [SerializeField]
        private string text = string.Empty;

        public override string ToString()
        {
            return string.Join("\n", new[] { PublishingOffice, ReportDateTime, TargetArea, HeadlineText, Text });
        }

        public Weather(string publishingOffice, string reportDateTime, string targetArea, string headlineText, string text)
        {
            this.publishingOffice = publishingOffice;
            this.reportDateTime = reportDateTime;
            this.targetArea = targetArea;
            this.headlineText = headlineText;
            this.text = text;
        }
    }
}