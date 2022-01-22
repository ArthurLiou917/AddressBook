using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO; //used for read and save file

namespace _2PersonalAddressBook
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dtpAdd.MaxDate = DateTime.Now;
            dtpEdit.MaxDate = DateTime.Now;
        }
        PersonalInfo[] OrganizerArray = new PersonalInfo[100]; //initialize main array for storing PersonalInfo
        int iCurrentIndex = 0; //Current index for whole program
        public struct PersonalInfo //structure for PersonalInfo consisting of full name,number,email,dob
        {
            public string FirstName;
            public string LastName;
            public string Number;
            public string Email;
            public int Month;
            public int Day;
            public int Year;
        }



        public int FindPerson(string FirstName, string LastName)//method for finding 
        {
            for (int i = 0; i < iCurrentIndex; i = i + 1)
            {
                if (OrganizerArray[i].FirstName.ToLower() == FirstName.ToLower() && OrganizerArray[i].LastName.ToLower() == LastName.ToLower())
                {
                    return i;//finds the location of the search inquiry
                }
            }
            return -1; //-1 is returned if there the search inquiry is unable to find person
        }
        public int FindAge(DateTime DateOfBirth)//method for finding age - used in show today birthday
        {
            int age = 0;
            age = DateTime.Now.Year - DateOfBirth.Year;
            return age;
        }

        public void Reset()//method for resetting textboxes
        {
            txtFirstNameEdit.Enabled = true;
            txtLastNameEdit.Enabled = true;

            dtpEdit.Enabled = false;
            txtEmailEdit.Enabled = false;
            txtNumEdit.Enabled =   false;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnDelete.Enabled = false;

            gBoxContact.Visible = false;
        }
        public void Clearall() //method for clearing all text boxes,listbox,etc. 
        {
            txtFirstName.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            txtEmail.Clear();
            txtNumber.Clear();
            dtpAdd.ResetText();
            txtFirstNameInfo.Clear();
            txtLastNameInfo.Clear();
            txtFirstNameEdit.Clear();
            txtLastNameEdit.Clear();
            txtNumEdit.Clear();
            txtEmailEdit.Clear();
            dtpEdit.ResetText();
            lb1.Items.Clear();
            Reset();
        }


        public string DisplayName(string FirstName, string LastName)//method for capitalizing first letter of First Name and Last Name
        {
            return FirstName.Substring(0, 1).ToUpper() + FirstName.Substring(1).ToLower() + " " + LastName.Substring(0, 1).ToUpper() + LastName.Substring(1).ToLower();
        }

        private void btnAdd_Click(object sender, EventArgs e)//Add contact button
        {
            string FirstName, LastName, Number, Email;
            int index;

            if (iCurrentIndex >= 100)//If current index is full from 
            {
                MessageBox.Show("The memory of this program is full");
                return;
            }
            else
            {
                FirstName = txtFirstName.Text.Trim();
                LastName = txtLastName.Text.Trim();
                Email = txtEmail.Text.Trim();
                Number = txtNumber.Text.Trim();


                if (FirstName == "" || LastName == "" || Email == "" || Number == "") //Ensures user does not leave open fields
                {
                    MessageBox.Show("Please fill in all required fields");
                    return;
                }

                else
                {
                    index = FindPerson(FirstName, LastName); //uses find person array to see if person is already in the database

                    if (index == -1) //person is not in database and is added
                    {
                        OrganizerArray[iCurrentIndex].FirstName = txtFirstName.Text.Trim();
                        OrganizerArray[iCurrentIndex].LastName = txtLastName.Text.Trim();
                        OrganizerArray[iCurrentIndex].Number = txtNumber.Text.Trim();
                        OrganizerArray[iCurrentIndex].Email = txtEmail.Text.Trim();
                        OrganizerArray[iCurrentIndex].Month = dtpAdd.Value.Month;
                        OrganizerArray[iCurrentIndex].Day = dtpAdd.Value.Day;
                        OrganizerArray[iCurrentIndex].Year = dtpAdd.Value.Year;

                        iCurrentIndex = iCurrentIndex + 1;//updates the current index

                        MessageBox.Show(DisplayName(FirstName, LastName) + " ----->Added");

                        Clearall();
                    }
                    else //person cannot be added because they are already in the database
                    {
                        MessageBox.Show("This person is already in the index");
                    }
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string FirstName, LastName, Number, Email;
            int index, day, month, year;

            FirstName = txtFirstNameInfo.Text.Trim();//trim the name
            LastName = txtLastNameInfo.Text.Trim();

            if (FirstName == "" || LastName == "")//Makes sure user puts in first and last name
            {
                MessageBox.Show("Please put in first name and last name");
                return;
            }
            else
            {
                index = FindPerson(FirstName, LastName);//uses find person array to determine if person is in the database

                if (index == -1)//person not in database
                {
                    MessageBox.Show("This person is not in the program");
                    gBoxContact.Visible = false;
                }

                else //person in database and displays information
                {
                    day = OrganizerArray[index].Day;
                    month = OrganizerArray[index].Month;
                    year = OrganizerArray[index].Year;
                    Number = OrganizerArray[index].Number;
                    Email = OrganizerArray[index].Email;

                    txtNumberInfo.Text = Number;
                    txtEmailInfo.Text = Email;
                    txtDateSearch.Text = month + "/" + day + "/" + year;
                    gBoxContact.Visible = true;//makes information group box visible if person is found
                }
            }

        }

        private void btnSearchToday_Click(object sender, EventArgs e)
        {
            string Fullname;
            int counter, year, month, day, age;
            counter = 0;
            DateTime dob;
            lb1.Items.Clear();
            for (int i = 0; i < iCurrentIndex; i = i +1)//Goes through databse to see if there are any matches for current "today" and "month"
            {
                if (OrganizerArray[i].Day == DateTime.Now.Day && OrganizerArray[i].Month == DateTime.Now.Month)
                {
                    year = OrganizerArray[i].Year;
                    month = OrganizerArray[i].Month;
                    day = OrganizerArray[i].Day;
                    dob = new DateTime(year, month, day);
                    age = FindAge(dob);
                    Fullname = DisplayName(OrganizerArray[i].FirstName, OrganizerArray[i].LastName);
                    lb1.Items.Add(Fullname + " turns " + age + "  today.");//displays full name and what age they turn
                    counter = counter + 1;//counter makes sure all matching dates are listed out.
                }
            }
            if (counter == 0)//if counter is 0 there are no matching dates
            {
                MessageBox.Show("There is no birthday today");
            }
        }

        private void btnCancelAdd_Click(object sender, EventArgs e)//cancel button in add tab
        {
            Clearall();
        }

        private void btnCancelSearch_Click(object sender, EventArgs e)//cancel button in search tab
        {
            Clearall();
        }

        private void btnReset_Click(object sender, EventArgs e)//reset button in showtodaybirthday tab
        {
            lb1.Items.Clear();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)//cancel button in edit tab
        {
            Clearall();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)//save toolstrip using txt files
        {
            SaveFileDialog saveDialog1 = new SaveFileDialog();
            string FileName;
            saveDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";//uses txt files

            try//try and catch for error in saving files
            {
                    if (saveDialog1.ShowDialog() == DialogResult.OK)
                    {
                        FileName = saveDialog1.FileName;
                        TextWriter tw = new StreamWriter(FileName);//creates txt writer
                        for (int i = 0; i < iCurrentIndex; i = i + 1)//where to write down array info
                        {
                            tw.Write(OrganizerArray[i].FirstName + ",");//commas splits input
                            tw.Write(OrganizerArray[i].LastName + ",");
                            tw.Write(OrganizerArray[i].Number + ",");
                            tw.Write(OrganizerArray[i].Email + ",");
                            tw.Write(OrganizerArray[i].Day.ToString() + ",");
                            tw.Write(OrganizerArray[i].Month.ToString() + ",");
                            tw.WriteLine(OrganizerArray[i].Year.ToString());
                        }
                        tw.Close();//close after while loop is finished
                    }
            }
            catch
            {
                MessageBox.Show("ERROR! Please use the right file.");
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strinput;
            string[] splittedinput;

            try//try and catch for error in opening files
            {
                OpenFileDialog opf = new OpenFileDialog();
                opf.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";//txt files only

                if (opf.ShowDialog() == DialogResult.OK)
                {
                    string filename;
                    filename = opf.FileName;

                    TextReader tr = new StreamReader(filename);//create a text reader
                    iCurrentIndex = 0;
                    while ((strinput = tr.ReadLine()) != null) //fills up array databse with input from txt files
                    {
                        splittedinput = strinput.Split(',');//comma is used to identify the different values
                        OrganizerArray[iCurrentIndex].FirstName = splittedinput[0];
                        OrganizerArray[iCurrentIndex].LastName = splittedinput[1];
                        OrganizerArray[iCurrentIndex].Number = splittedinput[2];
                        OrganizerArray[iCurrentIndex].Email = splittedinput[3];
                        OrganizerArray[iCurrentIndex].Day = Convert.ToInt32(splittedinput[4]);
                        OrganizerArray[iCurrentIndex].Month = Convert.ToInt32(splittedinput[5]);
                        OrganizerArray[iCurrentIndex].Year = Convert.ToInt32(splittedinput[6]);
                        iCurrentIndex = iCurrentIndex + 1;
                    }
                    tr.Close();//close reader
                }
            }
            catch
            {
                MessageBox.Show("ERROR! Please use the right file.");
            }
        }

        private void btnAuth_Click(object sender, EventArgs e)//makes sure user cannot mess around with edit tab
        {
            string FirstName, LastName;
            int index;

            FirstName = txtFirstNameEdit.Text.Trim();
            LastName = txtLastNameEdit.Text.Trim();

            if (FirstName == "" || LastName == "")//user has to put in both names
            {
                MessageBox.Show("Please put in first name and last name");
                return;
            }
            else
            {
                index = FindPerson(FirstName, LastName);//search if person is in database

                if (index == -1)//person is not in program
                {
                    MessageBox.Show("This person is not in the program");
                }
                else//prevents user from changing name once authenicated
                {
                    txtFirstNameEdit.Enabled = false;
                    txtLastNameEdit.Enabled = false;

                    dtpEdit.Enabled = true;
                    txtEmailEdit.Enabled = true;
                    txtNumEdit.Enabled = true;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnDelete.Enabled = true;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)//save/edit information button
        {
            int index;
            string FirstName, LastName,Email,Number;

            if (MessageBox.Show("Do you wish to save?", "Message", MessageBoxButtons.YesNo) == DialogResult.Yes)//Message box with option to select yes or no
            {
                FirstName = txtFirstNameEdit.Text.Trim();
                LastName = txtLastNameEdit.Text.Trim();
                Email = txtEmailEdit.Text.Trim();
                Number = txtNumEdit.Text.Trim();

                index = FindPerson(FirstName, LastName);//finds index of person

                if (Number == "" || Email == "")//some fields are empty
                {
                    MessageBox.Show("Please fill in all required fields");
                    return;
                }
                else//allows user to edit information of appropriate index
                {
                    OrganizerArray[index].Number = Number;
                    OrganizerArray[index].Email = Email;
                    OrganizerArray[index].Month = dtpEdit.Value.Month;
                    OrganizerArray[index].Day = dtpEdit.Value.Day;
                    OrganizerArray[index].Year = dtpEdit.Value.Year;

                    Clearall();

                    MessageBox.Show("Information is updated");
                }
            }
            else//nothing happens when you select no on message box
            {
                return;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string FirstName, LastName;
            int index;

            if(MessageBox.Show("Do you wish to delete?","Message",MessageBoxButtons.YesNo)==DialogResult.Yes)//Message box with option to select yes or no
            {
                FirstName = txtFirstNameEdit.Text.Trim();
                LastName = txtLastNameEdit.Text.Trim();

                index = FindPerson(FirstName, LastName);//finds index of person that needs to be deleted
                for (int i = index; i < iCurrentIndex; i = i + 1)//finds person in database using index and replaces them with other arrays essentially shifting arrays down one slot
                {
                    OrganizerArray[i].FirstName = OrganizerArray[i + 1].FirstName;
                    OrganizerArray[i].LastName = OrganizerArray[i + 1].LastName;
                    OrganizerArray[i].Number = OrganizerArray[i + 1].Number;
                    OrganizerArray[i].Email = OrganizerArray[i + 1].Email;
                    OrganizerArray[i].Month = OrganizerArray[i + 1].Month;
                    OrganizerArray[i].Day = OrganizerArray[i + 1].Day;
                    OrganizerArray[i].Year = OrganizerArray[i + 1].Year;
                }
                iCurrentIndex = iCurrentIndex - 1;//current index is updated

                Clearall();

                MessageBox.Show(DisplayName(FirstName,LastName)+ " is deleted");
            }
            else//nothing happens when you select no on message box
            {
                return;
            }
        }

        private void saveBinaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog1 = new SaveFileDialog();
            string FileName;

            try //try and catch for error in saving binary files
            {
                    saveDialog1.Filter = "binary files (*.bdo)|*.bdo|All files (*.*)|*.*";//uses binary files
                    if (saveDialog1.ShowDialog() == DialogResult.OK)
                    {
                        FileName = saveDialog1.FileName;//creating binary writer
                        FileStream fs = new FileStream(FileName, FileMode.Create);
                        BinaryWriter binaryWriter = new BinaryWriter(fs);

                        for (int i = 0; i < iCurrentIndex; i = i + 1)//writes array information into binary file
                        {
                            binaryWriter.Write(OrganizerArray[i].FirstName);
                            binaryWriter.Write(OrganizerArray[i].LastName);
                            binaryWriter.Write(OrganizerArray[i].Number);
                            binaryWriter.Write(OrganizerArray[i].Email);
                            binaryWriter.Write(OrganizerArray[i].Month);
                            binaryWriter.Write(OrganizerArray[i].Day);
                            binaryWriter.Write(OrganizerArray[i].Year);
                        }
                        binaryWriter.Flush();
                        binaryWriter.Close();//close when writer is done
                    }
            }
            catch
            {
                MessageBox.Show("ERROR! Please use the right file.");
                return;
            }
        }

        private void openBinaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog1 = new OpenFileDialog();
            string FileName;
            long length;

            try //try and catch for error in opening files
            {
                openDialog1.Filter = "binary files (*.bdo)|*.bdo|All files (*.*)|*.*";//reads only binary files
                if (openDialog1.ShowDialog() == DialogResult.OK)
                {
                    FileName = openDialog1.FileName;//creates binary reader
                    FileStream fs = new FileStream(FileName, FileMode.Open);
                    BinaryReader binaryReader = new BinaryReader(fs);

                    iCurrentIndex = 0;
                    length = binaryReader.BaseStream.Length;//sets length for binary reader
                    while (fs.Position < length)//reads information from binary files
                    {
                        OrganizerArray[iCurrentIndex].FirstName = binaryReader.ReadString();
                        OrganizerArray[iCurrentIndex].LastName = binaryReader.ReadString();
                        OrganizerArray[iCurrentIndex].Number = binaryReader.ReadString();
                        OrganizerArray[iCurrentIndex].Email = binaryReader.ReadString();
                        OrganizerArray[iCurrentIndex].Month = binaryReader.ReadInt32();
                        OrganizerArray[iCurrentIndex].Day = binaryReader.ReadInt32();
                        OrganizerArray[iCurrentIndex].Year = binaryReader.ReadInt32();
                        iCurrentIndex = iCurrentIndex + 1;
                    }
                    binaryReader.Close();//close reader when done
                }
            }
            catch
            {
                MessageBox.Show("ERROR! Please use the right file.");
                return;
            }
        }

        private void resetAddressBookToolStripMenuItem_Click(object sender, EventArgs e)//resets whole current address book
        {
            if (MessageBox.Show("Do you wish to reset?", "Message", MessageBoxButtons.YesNo) == DialogResult.Yes)//reset all the information in the database
            {
                iCurrentIndex = 0;
                Clearall();
                MessageBox.Show("Address book is reset");
            }
            else
            {
                return;
            }
        }
    }
}
