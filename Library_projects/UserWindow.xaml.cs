using Library_project.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Library_project
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window, INotifyPropertyChanged
    {
        public List<Categorybook> CategoryList { get; set; }
        private int CategoryFilterId = 0;
        private User CurrentUser;
        private string SearchFilter = "";
        public UserWindow(User user)
        {
            InitializeComponent();
            DataContext = this;
            CurrentUser = user;
            using (var context = new LibraryContext())
            {
                BookList = context.Books.ToList();
                CategoryList = context.Categorybooks.ToList();
                CategoryList.Insert(0, new Categorybook { Title = "Все категории" });
            }
        }
        public Visibility IsAdmin
        {
            get
            {
                if (CurrentUser.Role == "admin")
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void Invalidate(string ComponentName = "BookList")
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(ComponentName));
        }
        public void LoadBooks()
        {
            using (LibraryContext db = new LibraryContext())
            {
                BookList = db.Books
                    .Include(x => x.CategoryNavigation)
                    .ToList();
            }

            Invalidate("BookList");
        }
        private void BooksGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Book selectedBook = BooksGrid.SelectedItem as Book;

            if (selectedBook != null)
            {
                EditBookWindow window = new EditBookWindow(selectedBook);
                window.ShowDialog();
                LoadBooks();
                Invalidate();

            }
        }
        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            EditBookWindow window = new EditBookWindow(null);
            window.ShowDialog();
            LoadBooks();
            Invalidate();
        }
        private int SortType = 0;
        private void SortTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortType = SortTypeComboBox.SelectedIndex;
            Invalidate();
        }
       
        public string[] SortList { get; set; } =
        {
            "Без сортировки",
            "Название A-Я",
            "Название Я-А",
            "Год новые",
            "Год старые"
        };
        private void SearchFilterTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            SearchFilter = SearchFilterTextBox.Text;
            Invalidate();
        }
        private void CategoryFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CategoryFilterId = (CategoryFilter.SelectedItem as Categorybook).Id;
            Invalidate();
        }

        private IEnumerable<Book> _BookList;
        public IEnumerable<Book> BookList
        {
            get
            {
                var Result = _BookList;

                // поиск
                if (SearchFilter != "")
                    Result = Result.Where(p =>
                        p.Name.IndexOf(SearchFilter, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        p.Avtor.IndexOf(SearchFilter, StringComparison.OrdinalIgnoreCase) >= 0);

                // фильтр категории
                if (CategoryFilterId > 0)
                    Result = Result.Where(p => p.Category == CategoryFilterId);

                // сортировка
                switch (SortType)
                {
                    case 1:
                        Result = Result.OrderBy(p => p.Name);
                        break;

                    case 2:
                        Result = Result.OrderByDescending(p => p.Name);
                        break;

                    case 3:
                        Result = Result.OrderByDescending(p => p.YearBook);
                        break;

                    case 4:
                        Result = Result.OrderBy(p => p.YearBook);
                        break;
                }

                return Result;
            }
            set
            {
                _BookList = value;
                Invalidate();
            }
        }
        private void SearchFilterTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchFilterTextBox.Text == "Поиск книги")
            {
                SearchFilterTextBox.Text = "";
                SearchFilterTextBox.Foreground = Brushes.Black;
            }
        }

        private void SearchFilterTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchFilterTextBox.Text))
            {
                SearchFilterTextBox.Text = "Поиск книги";
                SearchFilterTextBox.Foreground = Brushes.Gray;
            }
        }
    }
}