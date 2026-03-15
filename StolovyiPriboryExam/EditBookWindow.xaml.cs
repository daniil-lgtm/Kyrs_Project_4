using Library_project.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Library_project
{
    public partial class EditBookWindow : Window, INotifyPropertyChanged
    {
        public Book CurrentBook { get; set; }
        public string WindowTitle { get; set; }
        public List<Categorybook> CategoryList { get; set; }
        public List<Manufacturer> ManufacturerList { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public EditBookWindow(Book book)    
        {
            InitializeComponent();

            if (book == null)
            {
                CurrentBook = new Book();
                WindowTitle = "Добавление книги";
            }
            else
            {
                CurrentBook = book;
                WindowTitle = "Редактирование книги";
            }


            using (LibraryContext db = new LibraryContext())
            {
                CategoryList = db.Categorybooks.ToList();
                ManufacturerList = db.Manufacturers.ToList();
            }

            DataContext = this;
        }
        private void Invalidate(string ComponentName = "BookList")
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(ComponentName));
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем обязательные поля
            if (string.IsNullOrWhiteSpace(CurrentBook.Name) ||
                string.IsNullOrWhiteSpace(CurrentBook.Avtor) ||
                CurrentBook.YearBook == null ||
                CurrentBook.QuantityPages <= 0 ||
                CurrentBook.Quantity <= 0 ||
                CurrentBook.Category == 0 ||
                CurrentBook.Manufacturer == 0)
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; 
            }

            using (LibraryContext db = new LibraryContext())
            {
                if (CurrentBook.Id == 0)
                {
                    // Новая книга
                    db.Books.Add(CurrentBook);
                }
                else
                {
                    // Редактируем существующую
                    db.Books.Update(CurrentBook);
                }

                db.SaveChanges();
                Invalidate(); 
            }

            MessageBox.Show("Книга сохранена", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentBook.Id == 0)
            {
                MessageBox.Show("Эта книга ещё не сохранена, удалять нечего.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Подтверждение удаления
            var result = MessageBox.Show(
                $"Вы действительно хотите удалить книгу \"{CurrentBook.Name}\"?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                using (LibraryContext db = new LibraryContext())
                {
                    var bookToDelete = db.Books.Find(CurrentBook.Id);
                    if (bookToDelete != null)
                    {
                        db.Books.Remove(bookToDelete);
                        db.SaveChanges();
                        MessageBox.Show("Книга удалена.", "Удаление", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close(); 
                    }
                    else
                    {
                        MessageBox.Show("Книга не найдена в базе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}