using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace autosell_center.cls
{
    public class OrgNode
    {
        private string id;
        private string name;
        private string title;
        private Dictionary<string, string> children;

        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                title = value;
            }
        }

        public Dictionary<string, string> Children
        {
            get
            {
                return children;
            }

            set
            {
                children = value;
            }
        }
    }
}