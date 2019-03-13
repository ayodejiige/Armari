using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace armari
{
    [Register("ClassCollectionView")]
    public class ClassCollectionView : UICollectionView
    {
        public string id;
        public List<int> Ids { get; set; } = null;

        #region Constructors
        public ClassCollectionView(IntPtr handle) : base(handle)
        {
        }
        #endregion

        #region Override Methods
        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            //InitializeData(ClosetCollectionViewController.CurrentClass);
            //Ids = ClosetCollectionViewController.Ids;
            Ids = new List<int>();
            //ClosetCollectionViewController.Ids = new List<int>();
            Application.logger.Message("Loaded class collection view");

            // Initialize
            DataSource = new ClassCollectionSource(this);
            Delegate = new ClassCollectionDelegate(this);
        }
        #endregion

        public void InitializeData(String type)
        {
            var ids = Application.mh.GetWardrobe(type);

            if (ids == null)
            {
                ids = new List<int>();
            }
            string msg = "Items: \n";
            for (int n = 0; n < ids.Count; ++n)
            {
                msg += string.Format("\t{0}\n", ids[n]);
            }
            Application.logger.Message(msg);

            Ids = ids;
        }

        public void UpdateDataSource()
        {
            ClassCollectionSource dataSource = DataSource as ClassCollectionSource;
            dataSource.SetIds(Ids);
        }

        public static List<int> InitializeDataStatic(String type)
        {
            var ids = Application.mh.GetWardrobe(type);

            if (ids == null)
            {
                ids = new List<int>();
            }
            string msg = "Items: \n";
            for (int n = 0; n < ids.Count; ++n)
            {
                msg += string.Format("\t{0}\n", ids[n]);
            }
            Application.logger.Message(msg);

            return ids;
        }

        public int IdentifierForIndexPath(NSIndexPath indexPath)
        {
            return Ids[(int)indexPath.Item];
        }
    }
}
