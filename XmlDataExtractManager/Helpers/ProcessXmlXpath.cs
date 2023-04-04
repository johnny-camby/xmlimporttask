using Microsoft.BizTalk.XPath;
using System;
using System.Xml;

namespace XmlDataExtractManager.Helpers
{
    public static class ProcessXmlXpath
    {
        private static XmlNode _currentNode;

        public static string GetValueByXPathExpression(string strXPath, int iExpressionType = 1, XmlNode xmlNodeOptional = null)
        {
            string strBuffer = String.Empty;
            XmlNode xmlNodeQuery = _currentNode;

            if (xmlNodeOptional != null)
                xmlNodeQuery = xmlNodeOptional;
            if (iExpressionType == 1)
            {
                XmlNode xmlNodeTmp = xmlNodeQuery.SelectSingleNode(strXPath);
                if (xmlNodeTmp != null)
                    strBuffer = xmlNodeTmp.InnerText;
            }
            else if (iExpressionType == 2)
            {
                XmlNodeList xmlNodeListTmp = xmlNodeQuery.SelectNodes(strXPath);
                if (xmlNodeListTmp != null)
                    foreach (XmlNode xmlNodeTmp in xmlNodeListTmp)
                    {
                        if (!String.IsNullOrWhiteSpace(strBuffer)) strBuffer += "; ";
                        strBuffer += xmlNodeTmp.InnerText;
                    }
            }
            else if (iExpressionType == 3)
            {
                XmlNodeList xmlNodeListTmp = xmlNodeQuery.SelectNodes(strXPath);
                if (xmlNodeListTmp != null)
                    foreach (XmlNode xmlNodeTmp in xmlNodeListTmp)
                    {
                        if (!String.IsNullOrWhiteSpace(strBuffer)) strBuffer += "; ";
                        strBuffer += xmlNodeTmp.InnerText;
                    }
            }
            return strBuffer;
        }

        public static string GetValueByAwesomeXPathExpression(string xmlDoc, string xpath)
        {
            string extractedValue = string.Empty;

            XPathReader reader = new XPathReader(xmlDoc, xpath);

            while (reader.ReadUntilMatch())
            {
                if (reader.Match(extractedValue))
                {
                    extractedValue = reader.ReadString();
                }
            }
            return extractedValue;
        }
    }
}
