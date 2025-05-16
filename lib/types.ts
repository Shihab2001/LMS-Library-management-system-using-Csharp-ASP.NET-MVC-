// Book related types
export interface Book {
  id: number
  title: string
  author: string
  isbn: string
  category: string
  quantity: number
  available: number
  description?: string
  addedDate: string
  coverImage?: string
}

// Member related types
export interface Member {
  id: string
  firstName: string
  lastName: string
  email: string
  phone: string
  address: string
  memberType: "standard" | "student" | "senior" | "premium"
  registrationDate: string
  status: "Active" | "Inactive" | "Suspended"
  booksBorrowed: number
}

// Transaction related types
export interface Transaction {
  id: string
  bookId: number
  memberId: string
  bookTitle: string
  memberName: string
  issueDate: string
  dueDate: string
  returnDate: string | null
  status: "Issued" | "Overdue" | "Returned"
  fine: number | null
  condition?: string
}

// User related types
export interface User {
  id: number
  name: string
  email: string
  role: "admin" | "librarian" | "user"
  avatar?: string
}

// Reservation related types
export interface Reservation {
  id: string
  bookId: number
  memberId: string
  bookTitle: string
  memberName: string
  reservationDate: string
  status: "Pending" | "Fulfilled" | "Cancelled"
}

// Digital Book related types
export interface DigitalBook {
  id: number
  bookId: number
  title: string
  author: string
  filePath: string
  fileType: "PDF" | "EPUB" | "MOBI"
  fileSize: number
  uploadDate: string
}
