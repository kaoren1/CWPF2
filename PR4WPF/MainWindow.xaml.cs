using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PR4WPF
{
    public class Note
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }

        public Note(string name, string description, DateTime? date)
        {
            Name = name;
            Description = description;
            Date = date;
        }
    }

    public partial class MainWindow : Window
    {
        public List<Note> notes = new List<Note>();
        private List<Note> filteredNotes = new List<Note>(); // Для фильтрованных записей

        public MainWindow()
        {
            InitializeComponent();

            notes = DeSerTasks.Deserialize<List<Note>>() ?? new List<Note>();//Новый вызов метода


            List.ItemsSource = notes;
            List.DisplayMemberPath = "Name";
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Note newNote = new Note(Name.Text, Description.Text, Date.SelectedDate);
            notes.Add(newNote);

            DeSerTasks.Serialize(notes);
            List.ItemsSource = notes;
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (List.SelectedItem != null)
            {
                Note selectedNote = (Note)List.SelectedItem;
                Name.Text = selectedNote.Name;
                Description.Text = selectedNote.Description;
            }
        }

        private void Date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (Note note in notes)//Цикл вместо Linq-запроса
            {
                if (note.Date == Date.SelectedDate)
                {
                    filteredNotes.Add(note);
                }
            }

            List.ItemsSource = filteredNotes;//фильтрованные записи
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (List.SelectedItem != null)
            {
                Note selectedNote = (Note)List.SelectedItem;

                // Обновляем выбранную заметку
                selectedNote.Name = Name.Text;
                selectedNote.Description = Description.Text;
                selectedNote.Date = Date.SelectedDate;

                // Сохраняем изменения в списке
                DeSerTasks.Serialize(notes);

                // Обновляем отображение списка
                List.ItemsSource = null;
                List.ItemsSource = notes;
            }
        }

    }

    public class DeSerTasks
    {
        public static void Serialize<T>(T list)
        {
            string json = JsonConvert.SerializeObject(list);
            File.WriteAllText("Note.json", json);
        }

        public static T Deserialize<T>()
        {
            if (File.Exists("Note.json"))//Проверка
            {
                string json = File.ReadAllText("Note.json");
                T notes1 = JsonConvert.DeserializeObject<T>(json);
                return notes1;
            }
            return default(T);
        }
    }
}
