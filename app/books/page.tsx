import Link from "next/link"
import { BookPlus, Filter, Search } from "lucide-react"

import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import { Badge } from "@/components/ui/badge"

export default function BooksPage() {
  return (
    <div className="container mx-auto py-6">
      <div className="flex items-center justify-between mb-6">
        <h1 className="text-3xl font-bold tracking-tight">Books Management</h1>
        <Link href="/books/add">
          <Button>
            <BookPlus className="mr-2 h-4 w-4" />
            Add New Book
          </Button>
        </Link>
      </div>

      <div className="flex flex-col md:flex-row gap-4 mb-6">
        <div className="relative flex-1">
          <Search className="absolute left-2.5 top-2.5 h-4 w-4 text-muted-foreground" />
          <Input type="search" placeholder="Search by title, author, or ISBN..." className="pl-8 w-full" />
        </div>
        <div className="flex gap-2">
          <Select defaultValue="all">
            <SelectTrigger className="w-[180px]">
              <SelectValue placeholder="Category" />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="all">All Categories</SelectItem>
              <SelectItem value="fiction">Fiction</SelectItem>
              <SelectItem value="non-fiction">Non-Fiction</SelectItem>
              <SelectItem value="science">Science</SelectItem>
              <SelectItem value="history">History</SelectItem>
              <SelectItem value="biography">Biography</SelectItem>
            </SelectContent>
          </Select>
          <Button variant="outline" size="icon">
            <Filter className="h-4 w-4" />
          </Button>
        </div>
      </div>

      <div className="rounded-md border">
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Title</TableHead>
              <TableHead>Author</TableHead>
              <TableHead>ISBN</TableHead>
              <TableHead>Category</TableHead>
              <TableHead className="text-center">Available</TableHead>
              <TableHead className="text-center">Total</TableHead>
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {books.map((book) => (
              <TableRow key={book.id}>
                <TableCell className="font-medium">{book.title}</TableCell>
                <TableCell>{book.author}</TableCell>
                <TableCell>{book.isbn}</TableCell>
                <TableCell>
                  <Badge variant="outline">{book.category}</Badge>
                </TableCell>
                <TableCell className="text-center">{book.available}</TableCell>
                <TableCell className="text-center">{book.total}</TableCell>
                <TableCell className="text-right">
                  <div className="flex justify-end gap-2">
                    <Button variant="outline" size="sm">
                      View
                    </Button>
                    <Button variant="outline" size="sm">
                      Edit
                    </Button>
                  </div>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </div>
    </div>
  )
}

const books = [
  {
    id: 1,
    title: "To Kill a Mockingbird",
    author: "Harper Lee",
    isbn: "9780061120084",
    category: "Fiction",
    available: 3,
    total: 5,
  },
  {
    id: 2,
    title: "1984",
    author: "George Orwell",
    isbn: "9780451524935",
    category: "Fiction",
    available: 2,
    total: 4,
  },
  {
    id: 3,
    title: "The Great Gatsby",
    author: "F. Scott Fitzgerald",
    isbn: "9780743273565",
    category: "Fiction",
    available: 1,
    total: 3,
  },
  {
    id: 4,
    title: "Sapiens: A Brief History of Humankind",
    author: "Yuval Noah Harari",
    isbn: "9780062316097",
    category: "Non-Fiction",
    available: 4,
    total: 4,
  },
  {
    id: 5,
    title: "A Brief History of Time",
    author: "Stephen Hawking",
    isbn: "9780553380163",
    category: "Science",
    available: 0,
    total: 2,
  },
  {
    id: 6,
    title: "The Immortal Life of Henrietta Lacks",
    author: "Rebecca Skloot",
    isbn: "9781400052189",
    category: "Biography",
    available: 2,
    total: 2,
  },
  {
    id: 7,
    title: "Educated",
    author: "Tara Westover",
    isbn: "9780399590504",
    category: "Biography",
    available: 1,
    total: 3,
  },
]
