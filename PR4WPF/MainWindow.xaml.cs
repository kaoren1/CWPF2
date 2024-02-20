using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace PR4WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class Note
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public Note(string title, string description, DateTime? date)
        {
            Title = title;
            Description = description;
            Date = date;
        }
    }
    public partial class MainWindow : Window
    {
        public List<Note> notes = new();
        public MainWindow()
        {
            InitializeComponent();
            notes = new List<Note>();
            if (File.Exists("Note.json"))
                notes = Deser.Deserialize<List<Note>>();
            List.ItemsSource = notes;
            List.DisplayMemberPath = "Name";
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (notes == null)
            {
                notes = new List<Note>();
            }
            Note note = new Note(Name.Text, Description.Text, Date.SelectedDate);
            notes.Add(note);
            List.ItemsSource = notes;
            Deser.Serialize(notes);
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (List.SelectedItem != null)
            {
                Note selectedNote = (Note)List.SelectedItem;
                Name.Text = selectedNote.Title;
                Description.Text = selectedNote.Description;
            }
        }

        private void Date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (Note item in notes)
            {
                if (item.Date == Date.SelectedDate)
                {
                    Name.Text = item.Title;
                    Description.Text = item.Description;
                }
            }
        }
    }
    public class Deser
    {
        public static void Serialize<T>(T list)
        {
            string json = JsonConvert.SerializeObject(list);
            File.WriteAllText("Note.json", json);
        }
        public static T Deserialize<T>()
        {
            string json = File.ReadAllText("Note.json");
            T Note = JsonConvert.DeserializeObject<T>(json);
            return Note;
        }
    }
}
