using AdaptiveUIPivot.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Html;
using Windows.Web.Syndication;

namespace AdaptiveUIPivot.ViewModels
{
    class FeedsViewModel : INotifyPropertyChanged
    {
        
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property. 
        // The CallerMemberName attribute that is applied to the optional propertyName 
        // parameter causes the property name of the caller to be substituted as an argument. 
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; NotifyPropertyChanged(); }
        }


        public ObservableCollection<FeedItem> NewsList { get; set; }

        //public ObservableCollection<FeedItem> FirstColumn { get; set; }


        //public ObservableCollection<FeedItem> SecondColumn { get; set; }

        //public ObservableCollection<FeedItem> ThirdColumn { get; set; }

        //public ObservableCollection<FeedItem> FourthColumn { get; set; }

        public FeedsViewModel()
        {
            NewsList = new ObservableCollection<FeedItem>();
            //FirstColumn = new ObservableCollection<FeedItem>();
            //SecondColumn = new ObservableCollection<FeedItem>();
            //ThirdColumn = new ObservableCollection<FeedItem>();
            //FourthColumn = new ObservableCollection<FeedItem>();

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            LoadData();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        public async Task LoadData()
        {
            IsLoading = true;
            SyndicationClient client = new SyndicationClient();
            var result = await client.RetrieveFeedAsync(new Uri("http://channel9.msdn.com/Feeds/RSS/mp4"));

            foreach (var r in result.Items.Take(12))
            {
                try
                {
                    NewsList.Add(new FeedItem
                    {
                        Title = r.Title.Text,
                        Description = HtmlUtilities.ConvertToText(r.Summary.Text),
                        ImageUrl = r.ElementExtensions.Last(e => e.NodeName == "thumbnail").AttributeExtensions[0].Value,
                    });
                }
                catch (Exception)
                {
                    continue;
                }
            }

            IsLoading = false;
        }

        //public async void ArrangeColumns(int numCol)
        //{
        //    if (NewsList.Count == 0)
        //        await LoadData();

        //    FirstColumn.Clear();
        //    SecondColumn.Clear();
        //    ThirdColumn.Clear();
        //    FourthColumn.Clear();

        //    if (numCol == 4)
        //    {
        //        AddToObservableCollection(NewsList.Take(3), FirstColumn);
        //        AddToObservableCollection(NewsList.Skip(3).Take(3), SecondColumn);
        //        AddToObservableCollection(NewsList.Skip(6).Take(3), ThirdColumn);
        //        AddToObservableCollection(NewsList.Skip(9).Take(3), FourthColumn);
        //    }
        //    else if (numCol == 3)
        //    {
        //        AddToObservableCollection(NewsList.Take(4), FirstColumn);
        //        AddToObservableCollection(NewsList.Skip(4).Take(4), SecondColumn);
        //        AddToObservableCollection(NewsList.Skip(8).Take(4), ThirdColumn);
        //    }
        //    else if (numCol == 2)
        //    {
        //        AddToObservableCollection(NewsList.Take(6), FirstColumn);
        //        AddToObservableCollection(NewsList.Skip(6).Take(6), SecondColumn);

        //    }
        //    else if (numCol == 1)
        //    {
        //        AddToObservableCollection(NewsList.Take(12), FirstColumn);
        //    }

        //}

        private void AddToObservableCollection(IEnumerable<FeedItem> list, ObservableCollection<FeedItem> collection)
        {
            foreach (var c in list)
            {
                collection.Add(c);
            }
        }
    }

}

