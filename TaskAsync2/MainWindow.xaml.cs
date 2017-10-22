using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace TaskAsync2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Guid currentId = Guid.Empty;
        UserRepository userRepository = new UserRepository();

        public MainWindow()
        {
            InitializeComponent();
            lblFirstName.Text = string.Empty;
            lblLastName.Text = string.Empty;
            lblAge.Text = string.Empty;
        }

        private async void btnRead_Click(object sender, RoutedEventArgs e)
        {
            User user = await userRepository.Read(currentId);

            if (user != null)
            {
                lblFirstName.Text = user.FirstName;
                lblLastName.Text = user.LastName;
                lblAge.Text = user.Age.ToString();

                txtLog.Text += $"{Environment.NewLine } Reading of user {currentId} is finished ";
            }
            else
            {
                txtLog.Text += $"{Environment.NewLine } Cannot read user {currentId}.";
            }
        }

        private async void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            Guid currentGuid = currentId;
            int age;

            if (int.TryParse(txtAge.Text, out age))
            {
                currentId = await userRepository.Create(txtFirstName.Text, txtLastName.Text, age);
            }
            else
            {
                txtLog.Text += $"{Environment.NewLine } User age should be number.";
            }
            
            if (currentId != currentGuid)
            {
                txtLog.Text += $"{Environment.NewLine } User was created id: {currentId}";
            }
            else
            {
                txtLog.Text += $"{Environment.NewLine } User creation is failed.";
            }
        }

        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            bool result = false;
            int age;

            if (int.TryParse(txtAge.Text, out age))
            {
                result = await userRepository.Update(currentId, txtFirstName.Text, txtLastName.Text, age);
            }

            if (result)
            {
                txtLog.Text += $"{Environment.NewLine} User {currentId} was updated.";
            }
            else
            {
                txtLog.Text += $"{Environment.NewLine} User {currentId} updated is FAILED.";
            }         
        }

        private async void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            bool result = false;
            int age;

            if (int.TryParse(txtAge.Text, out age))
            {
                result = await userRepository.Remove(currentId);
            }

            if (result)
            {
                txtLog.Text += $"{Environment.NewLine } User {currentId} was removed.";
            }
            else
            {
                txtLog.Text += $"{Environment.NewLine } Cannot remove user {currentId}.";
            }
        }
    }

    class UserRepository
    {
        List<User> userContext = new List<User>();

        public async Task<Guid> Create(string firstName, string lastName, int age)
        {
            User user = new User()
            {
                Id = Guid.NewGuid(),
                FirstName = (!string.IsNullOrEmpty(firstName)) ? firstName : string.Empty,
                LastName = (!string.IsNullOrEmpty(lastName)) ? lastName : string.Empty,
                Age = age
            };
            await Task.Delay(500);
            userContext.Add(user);
            return user.Id;
        }

        public async Task<bool> Remove(Guid userId)
        {
            User user = await Read(userId);
            return userContext.Remove(user);
        }

        public async Task<User> Read(Guid userId)
        {
            await Task.Delay(5000);
            User user = userContext.FirstOrDefault(u => u.Id == userId);
            return user;
        }

        public async Task<bool> Update(Guid userId, string firstName, string lastName, int age)
        {
            User user = await Read(userId);
            user.FirstName = firstName;
            user.LastName = lastName;
            user.Age = age;
            return true;
        }
    }

    class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Age { get; set; }
    }
}
