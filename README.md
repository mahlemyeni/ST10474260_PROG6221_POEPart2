# ST10474260_PROG6221_POEP2
# Cybersecurity ChatBot (WPF) 


## Overview

The Cybersecurity Awareness Chatbot is a C# WPF desktop application designed to educate users about cybersecurity best practices through an interactive and user-friendly interface. The chatbot provides advice on topics such as password security, phishing awareness, privacy protection, and online scams while maintaining a conversational flow through memory recall and sentiment detection.

This project was developed as part of a Programming POE and demonstrates the use of:

* Windows Presentation Foundation (WPF)
* Object-Oriented Programming (OOP)
* Generic Collections
* Delegates and Event Handling
* User Interface Design
* Audio Integration
* Memory Management
* Sentiment Analysis
* GitHub Version Control
* Continuous Integration (CI)

---

#  Features

### Interactive GUI

* Modern WPF user interface
* Scrollable chat display
* Message timestamps
* User and chatbot message styling

### Voice Greeting

* Plays a welcome audio message when the application starts
* Replay greeting using the **"Speak"** button

### Cybersecurity Knowledge Base

The chatbot can provide information on:

* Password Security
* Phishing Attacks
* Privacy Protection
* Online Scams
* General Cybersecurity Awareness

### Keyword Recognition

Recognises keywords such as:

* Password
* Phishing
* Scam
* Privacy

and responds with relevant cybersecurity advice.

### Dynamic Responses

Uses collections and randomisation to provide different responses for the same topic, making conversations feel more natural.

### Memory and Recall

The chatbot can remember:

* User names
* User interests

and refer back to them later in the conversation.

### Sentiment Detection

Recognises emotions such as:

* Worried
* Curious
* Frustrated

and adjusts responses accordingly.

### Conversation Flow

Supports follow-up interactions including:

* Tell me more
* Another tip
* Explain more

allowing users to continue discussing a topic without restarting the conversation.

---

# 🏗️ Technologies Used

| Technology     | Purpose                   |
| -------------- | ------------------------- |
| C#             | Core Programming Language |
| WPF            | Graphical User Interface  |
| XAML           | User Interface Design     |
| .NET           | Application Framework     |
| GitHub         | Version Control           |
| GitHub Actions | Continuous Integration    |
| SoundPlayer    | Audio Playback            |

---

# 📂 Project Structure

```text
Chatbot_WPF/
│
├── MainWindow.xaml
├── MainWindow.xaml.cs
├── AudioPlayer.cs
├── ChatMessage.cs
├── Converters.cs
├── greeting.wav
├── App.xaml
├── App.xaml.cs
│
├── Properties/
│
└── .github/
    └── workflows/
        └── dotnet.yml
```

---

## Clone Repository

```bash
git clone https://github.com/YOUR_USERNAME/Cybersecurity-Awareness-Chatbot.git
```

---

## Open Project

1. Launch Visual Studio
2. Open the solution file (.sln)
3. Build the project

```bash
Build > Build Solution
```

4. Run the application

```bash
Debug > Start Debugging
```

---

# Audio Setup

The chatbot uses a WAV file for its voice greeting.

Place:

```text
greeting.wav
```

inside the project directory.

Ensure the file properties are:

```text
Build Action: Content
Copy to Output Directory: Copy if newer
```

---

# Example Interaction

```text
Bot: Welcome to the Cybersecurity Awareness Chatbot!

User: My name is Amahle

Bot: Nice to meet you Amahle! I'll remember your name.

User: Tell me about password security

Bot: Use strong passwords with symbols and numbers.

User: Tell me more

Bot: Enable two-factor authentication whenever possible.

User: What is my name?

Bot: Your name is Amahle.
```

---

# Assignment Requirements Covered

## Part 1

* Voice Greeting
* ASCII Art Display
* User Interaction
* Cybersecurity Responses
* Input Validation
* Console UI Enhancements
* GitHub Version Control

## Part 2

* WPF GUI Application
* Dynamic Responses
* Keyword Recognition
* Generic Collections
* Memory and Recall
* Sentiment Detection
* Delegates and Event Handling
* Enhanced User Experience

---

# Continuous Integration

GitHub Actions is used to automatically build the project whenever code is pushed to the repository.

Workflow file:

```text
.github/workflows/dotnet.yml
```

CI verifies:

* Successful compilation
* Project integrity
* Build consistency

---

# Future Improvements

Potential future enhancements include:

* Speech-to-Text Recognition
* Text-to-Speech Responses
* Additional Cybersecurity Topics
* User Profiles
* Chat History Storage
* AI-powered Responses
* Dark Mode Theme
* Cybersecurity Quiz Feature

---
for guidance and learning materials used throughout the development of this project.

