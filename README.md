# TaskStreamPro — Hệ thống Quản lý Công việc Nhóm

TaskStreamPro là một ứng dụng web quản lý công việc và dự án theo mô hình Kanban, được phát triển như một sản phẩm SaaS hiện đại. Hệ thống hỗ trợ làm việc nhóm, phân quyền linh hoạt, giao diện trực quan và thời gian thực.

![UI Preview](screenshots/dashboard.png)

## Mục lục

- [Tính năng chính](#tính-năng-chính)
- [Phân quyền](#phân-quyền)
- [Công nghệ sử dụng](#công-nghệ-sử-dụng)
- [Kiến trúc cơ sở dữ liệu](#kiến-trúc-cơ-sở-dữ-liệu)
- [Cài đặt và chạy](#cài-đặt-và-chạy)
- [Cấu trúc thư mục](#cấu-trúc-thư-mục)
- [API Tổng quan](#api-tổng-quan)
- [Hướng phát triển](#hướng-phát-triển)
- [Tác giả](#tác-giả)

---

## Tính năng chính

- **Xác thực người dùng**: Đăng ký, đăng nhập, quên mật khẩu.
- **Dashboard cá nhân**: Tổng quan công việc, biểu đồ thống kê (tổng project, task theo trạng thái, quá hạn).
- **Quản lý dự án**:
  - Tạo, sửa, xóa project (chỉ user thường cũng có thể tạo project riêng).
  - Danh sách project dạng card, hiển thị số task, thành viên, trạng thái.
- **Kanban Board**:
  - 3 cột: To Do, In Progress, Done.
  - Thẻ công việc hiển thị tiêu đề, mức ưu tiên (Low/Medium/High), deadline, avatar người được gán.
  - Kéo thả chuyển cột, sắp xếp thứ tự (drag & drop).
  - Lọc theo priority, người được gán, deadline.
- **Quản lý Task**:
  - Tạo, sửa, xóa task qua modal form.
  - Gán task cho thành viên (phụ thuộc quyền).
- **Chat thời gian thực**: Trong mỗi project có kênh chat nhóm, hiển thị avatar, timestamp, real-time bằng SignalR.
- **Giao diện chuyên nghiệp**: Clean, minimal, responsive, hỗ trợ light/dark mode.

---

## Phân quyền

### Vai trò toàn hệ thống (`system_role` trong bảng `users`)
- **Admin**: Toàn quyền quản lý hệ thống, xem và can thiệp mọi project, quản lý người dùng.
- **User**: Người dùng thông thường, có thể tự tạo project và trở thành leader của project đó. Khi được thêm vào project của người khác, có thể là leader hoặc member.

### Vai trò trong project (`project_role` trong bảng `project_members`)
- **Leader**: Quản lý toàn bộ project – sửa thông tin, thêm/xóa thành viên, gán task cho bất kỳ ai, xóa task.
- **Member**: Chỉ xem board, tạo task mới (chỉ tự gán hoặc để trống), sửa/xóa task do mình tạo, kéo thả đổi trạng thái mọi task. Không được gán task cho người khác.

> Xem chi tiết ma trận quyền ở phần [Phân quyền chi tiết](docs/authorization.md) (có thể thêm file docs nếu cần).

---

## Công nghệ sử dụng

- **Frontend**: React (Vite), React Router, React Beautiful DnD (kéo thả), Axios, Context API, Tailwind CSS hoặc Shadcn UI.
- **Backend**: ASP.NET Core Web API (.NET 8), Entity Framework Core, SignalR (real-time chat).
- **Database**: PostgreSQL (dùng UUID cho khóa chính).
- **Authentication**: JWT (JSON Web Tokens).
- **Deployment**: Docker (tuỳ chọn).

---

## Kiến trúc cơ sở dữ liệu

![Database Schema](screenshots/db-schema.png) *(có thể vẽ bằng dbdiagram.io)*

Sơ đồ khái niệm:

- **users**: id, email, password_hash, full_name, avatar_url, system_role (admin/user), created_at, updated_at.
- **projects**: id, name, description, status (active/archived), created_by (FK users), timestamps.
- **project_members**: (project_id, user_id) PK, project_role (leader/member), joined_at.
- **tasks**: id, project_id, title, description, priority (Low/Medium/High), status (ToDo/InProgress/Done), deadline, assignee_id, sort_order, timestamps.
- **messages**: id, project_id, sender_id, content, created_at.

Luồng chính: User tạo project → tự động thêm vào project_members với role = leader → Thêm thành viên khác (member hoặc leader). Tasks liên kết với project và người được assign.

---

## Cài đặt và chạy

### Yêu cầu

- .NET 8 SDK
- Node.js 18+
- PostgreSQL 14+
- Git

### Backend

```bash
git clone https://github.com/your-username/taskflow.git
cd taskflow/backend
# Tạo file appsettings.Development.json với connection string PostgreSQL
dotnet restore
dotnet ef database update
dotnet run
```

### Frontend

```bash
cd taskflow/frontend
npm install
npm run dev
```

### Seed dữ liệu

Admin mặc định: `admin@taskflow.com` / `Admin123!` (có thể thay đổi trong seeder).

---

## Cấu trúc thư mục

```
taskflow/
├── backend/
│   ├── Controllers/
│   ├── Models/
│   ├── DTOs/
│   ├── Data/ (DbContext, Migrations)
│   ├── Services/
│   ├── Hubs/ (ChatHub)
│   ├── Middleware/
│   └── Program.cs
├── frontend/
│   ├── src/
│   │   ├── components/ (Auth, Dashboard, Kanban, Chat...)
│   │   ├── pages/
│   │   ├── context/
│   │   ├── services/ (API calls, SignalR)
│   │   └── App.tsx
│   └── public/
├── docs/ (tài liệu, screenshots)
├── README.md
└── docker-compose.yml (tuỳ chọn)
```

---

## API Tổng quan

| Endpoint                     | Method | Mô tả |
|------------------------------|--------|-------|
| /api/auth/register           | POST   | Đăng ký |
| /api/auth/login              | POST   | Đăng nhập (trả JWT) |
| /api/projects                | GET    | Danh sách project của user |
| /api/projects                | POST   | Tạo project mới |
| /api/projects/{id}           | GET    | Chi tiết project |
| /api/projects/{id}/members   | POST   | Thêm thành viên |
| /api/tasks?projectId=...     | GET    | Lấy danh sách task (hỗ trợ filter) |
| /api/tasks                   | POST   | Tạo task mới |
| /api/tasks/{id}              | PUT    | Cập nhật task (cả status, sort_order) |
| /api/tasks/{id}              | DELETE | Xóa task |
| /api/chat/{projectId}        | GET    | Lấy tin nhắn cũ |
| /api/chat/hub                | WebSocket | SignalR hub cho chat real-time |

---

## Hướng phát triển

- Thêm tính năng lịch (calendar view).
- Thêm thông báo real-time khi được assign task.
- Xuất báo cáo thống kê.
- Tích hợp thanh toán (SaaS multi-tenant).
- CI/CD pipeline.

---
**TaskStreamPro** — Đơn giản hoá quản lý công việc nhóm, sức mạnh của Kanban trong tầm tay bạn.
