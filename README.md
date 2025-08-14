# 🧠 CleverDesk – Your Smart Productivity Assistant

**CleverDesk** is a modern full-stack ASP.NET Core MVC web application that helps users manage notes, track tasks, and chat with a **Gemini 2.5 Pro AI assistant** — all in one intuitive interface.  

Built in just 4 days with precision and passion.

---

## 🚀 Features

### ✅ **Task Management**
- Create, update, complete, and delete tasks  
- Organize tasks by category  
- Prioritize and manage your to-do list effectively  

### 🗒️ **Notes & Notebooks**
- Create digital notebooks  
- Add and edit notes inside each notebook  
- Attach custom cover images to notebooks for better visuals  

### 💬 **AI Assistant (Powered by Google Gemini)**
- Chat with a built-in AI assistant  
- Ask and receive answers in real time  
- Saves chat history per user securely  

### 🔐 **Authentication & Authorization**
- User registration and login  
- Session-based authentication  
- Secure chat and data access per user  

---

## 🧑‍💻 Tech Stack
- **Frontend:** Razor Views, Bootstrap 5, HTML/CSS  
- **Backend:** ASP.NET Core MVC (.NET 8+)  
- **Database:** Entity Framework Core + SQL Server  
- **AI API:** Google Gemini 2.5 flash via REST API integration  
- **Security:** Identity-based Authentication, Claims for UserId  
- **Deployment Ready:** Clean architecture and async DB calls  

---

## 🔁 App Flow

1. **User Registration/Login** – Users create an account and securely log in.  
2. **Home Dashboard** – Navigate between Notes, Tasks, and AI Assistant modules.  
3. **Notes Module** – Create notebooks → Add notes inside → Assign cover image → Organized per user.  
4. **Tasks Module** – Add task with name and status → Mark complete/incomplete → Track progress visually.  
5. **AI Assistant Module** – Send message → Gemini API processes → AI reply shown in real time → Chat stored in DB.  

---

## 🛠️ Setup Locally

### **Prerequisites**
- [.NET 8+ SDK](https://dotnet.microsoft.com/download)  
- SQL Server or LocalDB installed  
- Git installed  
- Google Gemini API key ([Get one here](https://aistudio.google.com/app/apikey))  

---

### **Steps to Setup**
1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/CleverDesk.git
   cd CleverDesk
2. **Set up connection string and api key**
-Add your sql server in appsettings.json and api key in GeminiService.cs
3. **Add migrations**
 ```bash
   add-migration initialcreation
   update-database
4. **Run**
