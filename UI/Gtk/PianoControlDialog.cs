using System;
using Gtk;
using GUI = Gtk.Builder.ObjectAttribute;

namespace Sanford.Multimedia.Midi.UI.Gtk
{
    public partial class PianoControlDialog : Window
    {
        [GUI] private Label noteRangeLabel = null;
        [GUI] private Button okButton = null;
        [GUI] private Button cancelButton = null;
        [GUI] private SpinButton lowNoteIDSpinButton;
        [GUI] private Box noteRangeBox;
        [GUI] private Label highNoteIDLabel;
        [GUI] private Label lowNoteIDLabel;
        [GUI] private SpinButton highNoteIDSpinButton;

        private int lowNoteID;

        private int highNoteID;

        public PianoControlDialog() : this(new Builder("PianoControlDialog.glade"))
        {
            InitializeComponent();

            UpdateProperties();
        }

        //       public PianoControlDialog() : this(new Builder("PianoControlDialog.glade")) { }

        private PianoControlDialog(Builder builder) : base(builder.GetRawOwnedObject("PianoControlDialog"))
        {
            builder.Autoconnect(this);

            DeleteEvent += Window_DeleteEvent;

            okButton.Clicked += okButton_Clicked;
            cancelButton.Clicked += cancelButton_Clicked;
            lowNoteIDSpinButton.ValueChanged += new EventHandler(lowNoteIDSpinButton_ValueChanged);
            lowNoteIDSpinButton.SetRange(0, 127);
            highNoteIDSpinButton.ValueChanged += new EventHandler(lowNoteIDSpinButton_ValueChanged);
            highNoteIDSpinButton.SetRange(0, 127);
            noteRangeBox.Add(highNoteIDLabel);
            noteRangeBox.Add(lowNoteIDLabel);
            noteRangeBox.Add(highNoteIDSpinButton);
            noteRangeBox.Add(lowNoteIDSpinButton);
            /*lowNoteIDSpinButton.Clicked += SpinButton;
            noteRangeBox += Box;
            highNoteIDLabel += Label;
            lowNoteIDLabel += Label;
            highNoteIDSpinButton += SpinButton;
            noteRangeBox.SuspendLayout;*/
        }

        private void UpdateProperties()
        {
            lowNoteID = (int)lowNoteIDSpinButton.Value;
            highNoteID = (int)highNoteIDSpinButton.Value;
        }

        private void InitializeComponent()
        {

            okButton.Clicked += okButton_Clicked;
            cancelButton.Clicked += cancelButton_Clicked;
            lowNoteIDSpinButton.ValueChanged += new EventHandler(lowNoteIDSpinButton_ValueChanged);
            lowNoteIDSpinButton.SetRange(0, 127);
            highNoteIDSpinButton.ValueChanged += new EventHandler(lowNoteIDSpinButton_ValueChanged);
            highNoteIDSpinButton.SetRange(0, 127);
            noteRangeBox.Add(highNoteIDLabel);
            noteRangeBox.Add(lowNoteIDLabel);
            noteRangeBox.Add(highNoteIDSpinButton);
            noteRangeBox.Add(lowNoteIDSpinButton);
            /*lowNoteIDSpinButton.Clicked += SpinButton;
            noteRangeBox += Box;
            highNoteIDLabel += Label;
            lowNoteIDLabel += Label;
            highNoteIDSpinButton += SpinButton;
            noteRangeBox.SuspendLayout;*/
        }

        private void lowNoteIDSpinButton_ValueChanged(object sender, EventArgs e)
        {
            if (lowNoteIDSpinButton.Value > highNoteIDSpinButton.Value)
            {
                highNoteIDSpinButton.Value = lowNoteIDSpinButton.Value;
            }
        }

        private void highNoteIDSpinButton_ValueChanged(object sender, EventArgs e)
        {
            if (highNoteIDSpinButton.Value < lowNoteIDSpinButton.Value)
            {
                lowNoteIDSpinButton.Value = highNoteIDSpinButton.Value;
            }
        }

        

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }

        private void okButton_Clicked(object sender, EventArgs e)
        {
            UpdateProperties();

            Close();
        }

        private void cancelButton_Clicked(object sender, EventArgs e)
        {
            Close();
        }

        public int LowNoteID
        {
            get
            {
                return lowNoteID;
            }
            set
            {
                #region Require

                if (value < 0 || value > ShortMessage.DataMaxValue)
                {
                    throw new ArgumentOutOfRangeException("LowNoteID", value,
                        "Low note ID out of range.");
                }

                #endregion

                lowNoteID = value;

                lowNoteIDSpinButton.Value = value;

                if (lowNoteID > highNoteID)
                {
                    highNoteID = lowNoteID;
                    highNoteIDSpinButton.Value = highNoteID;
                }
            }
        }

        public int HighNoteID
        {
            get
            {
                return highNoteID;
            }
            set
            {
                #region Require

                if (value < 0 || value > ShortMessage.DataMaxValue)
                {
                    throw new ArgumentOutOfRangeException("HighNoteID", value,
                        "High note ID out of range.");
                }

                #endregion

                highNoteID = value;

                highNoteIDSpinButton.Value = value;

                if (highNoteID < lowNoteID)
                {
                    lowNoteID = highNoteID;
                    lowNoteIDSpinButton.Value = highNoteID;
                }
            }
        }

        /*
        private void InitializeComponent()
        {
            /*this.okButton = new Button();
            this.cancelButton = new Button();
            this.lowNoteIDSpinButton = new SpinButton();
            this.noteRangeBox = new Box();
            this.highNoteIDLabel = new Label();
            this.lowNoteIDLabel = new Label();
            this.highNoteIDSpinButton = new SpinButton();
            ((System.ComponentModel.ISupportInitialize)(this.lowNoteIDSpinButton)).BeginInit();
            this.noteRangeBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.highNoteIDSpinButton)).BeginInit();
            this.SuspendLayout();

        }

        /*
        private Button okButton;
        private Button cancelButton;
        private SpinButton lowNoteIDSpinButton;
        private Box noteRangeBox;
        private Label highNoteIDLabel;
        private Label lowNoteIDLabel;
        private SpinButton highNoteIDSpinButton;*/
        
    }
}
