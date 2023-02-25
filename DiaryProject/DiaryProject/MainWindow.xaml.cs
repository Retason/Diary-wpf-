using DiaryProject.Data;
using DiaryProject.Model;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DiaryProject
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Note> _notes;
        private DataSerializer<Note> _serializer;
        private DateTime _selectedDate;

        public MainWindow()
        {
            InitializeComponent();

            _selectedDate = DateTime.Today;
            DatePicker.SelectedDate = _selectedDate;

            try
            {
                _serializer = new DataSerializer<Note>("notes.json");
                _notes = _serializer.Deserialize();
            }
            catch (FileNotFoundException)
            {
                // Если файл не найден, создаем пустой список заметок
                _notes = new List<Note>();
            }
            catch (Exception ex)
            {
                // Если возникла другая ошибка, выводим сообщение об ошибке
                MessageBox.Show($"Ошибка при загрузке заметок: {ex.Message}");
            }

            RefreshNotesList();
        }

        private void RefreshNotesList()
        {
            if (_notes == null)
            {
                _notes = new List<Note>();
            }
            List<Note> notesForSelectedDate = _notes.Where(n => n.Date == _selectedDate).ToList();
            NotesList.ItemsSource = notesForSelectedDate;
        }

        private void OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedDate = DatePicker.SelectedDate ?? DateTime.Today;
            RefreshNotesList();
        }

        private void OnNoteSelected(object sender, SelectionChangedEventArgs e)
        {
            Note selectedNote = NotesList.SelectedItem as Note;
            if (selectedNote != null)
            {
                TitleTextBox.Text = selectedNote.Title;
                DescriptionTextBox.Text = selectedNote.Description;
            }
        }

        private void OnCreateButtonClick(object sender, RoutedEventArgs e)
        {
            //проверка на пустые поля
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text) || string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            Note newNote = new Note
            {
                Title = TitleTextBox.Text,
                Description = DescriptionTextBox.Text,
                Date = _selectedDate
            };
            _notes.Add(newNote);
            _serializer.Serialize(_notes);
            RefreshNotesList();
        }

        private void OnSaveButtonClick(object sender, RoutedEventArgs e)
        {
            Note selectedNote = NotesList.SelectedItem as Note;
            if (selectedNote != null)
            {
                selectedNote.Title = TitleTextBox.Text;
                selectedNote.Description = DescriptionTextBox.Text;
                _serializer.Serialize(_notes);
                RefreshNotesList();
            }
        }

        private void OnDeleteButtonClick(object sender, RoutedEventArgs e)
        {
            Note selectedNote = NotesList.SelectedItem as Note;
            if (selectedNote != null)
            {
                _notes.Remove(selectedNote);
                _serializer.Serialize(_notes);
                RefreshNotesList();
            }
        }
    }
}
