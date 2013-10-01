using System;
using System.Windows.Forms;
using Elide.Environment.Views;
using Elide.TextEditor;

namespace Elide.TextWorkbench.Views
{
	public sealed class BookmarkView : AbstractView, IBookmarkView
	{        
		public BookmarkView()
		{
			
		}
				
        public override void Activate()
        {
            Manager.TreeView.Select();
        }
		
		public override object Control 
		{
			get 
			{
				return Manager.TreeView; 
			}
		}

        private BookmarkManager _manager;
        internal BookmarkManager Manager
        {
            get
            {
                if (_manager == null)
                {
                    _manager = new BookmarkManager();
                    _manager.Initialize(App);
                }

                return _manager;
            }
        }
	}
}
