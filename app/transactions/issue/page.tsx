"use client"

import type React from "react"

import { useState } from "react"
import { useRouter } from "next/navigation"
import Link from "next/link"
import { ArrowLeft, Search } from "lucide-react"

import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { toast } from "@/components/ui/use-toast"
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "@/components/ui/card"
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"

export default function IssueBookPage() {
  const router = useRouter()
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [selectedBook, setSelectedBook] = useState<Book | null>(null)
  const [selectedMember, setSelectedMember] = useState<Member | null>(null)
  const [searchQuery, setSearchQuery] = useState("")
  const [memberSearchQuery, setMemberSearchQuery] = useState("")

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()

    if (!selectedBook || !selectedMember) {
      toast({
        title: "Error",
        description: "Please select both a book and a member",
        variant: "destructive",
      })
      return
    }

    setIsSubmitting(true)






    // // Simulate API call
    // setTimeout(() => {
    //   toast({
    //     title: "Book Issued Successfully",
    //     description: `"${selectedBook.title}" has been issued to ${selectedMember.name}.`,
    //   })
    //   setIsSubmitting(false)
    //   router.push("/transactions")
    // }, 1000)


    try {
      const response = await fetch("http://localhost:5000/api/IssuedBooksApi/issue", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          bookId: selectedBook.id,
          memberId: selectedMember.id,
        }),
      });
    
      if (!response.ok) {
        throw new Error("Failed to issue book");
      }
    
      const data = await response.json();
    
      toast({
        title: "Success",
        description: data.message || `"${selectedBook.title}" has been issued to ${selectedMember.name}.`,
      });
    
      router.push("/transactions");
    } catch (error) {
      console.error(error);
      toast({
        title: "Error",
        description: "Failed to issue the book. Please try again.",
        variant: "destructive",
      });
    } finally {
      setIsSubmitting(false);
    }
    
  }

  const filteredBooks = books.filter(
    (book) =>
      book.title.toLowerCase().includes(searchQuery.toLowerCase()) ||
      book.author.toLowerCase().includes(searchQuery.toLowerCase()) ||
      book.isbn.includes(searchQuery),
  )

  const filteredMembers = members.filter(
    (member) =>
      member.name.toLowerCase().includes(memberSearchQuery.toLowerCase()) ||
      member.id.toLowerCase().includes(memberSearchQuery.toLowerCase()),
  )

  return (
    <div className="container mx-auto py-6">
      <div className="mb-6">
        <Link href="/transactions" className="flex items-center text-sm text-muted-foreground hover:text-primary">
          <ArrowLeft className="mr-2 h-4 w-4" />
          Back to Transactions
        </Link>
      </div>

      <Card className="mx-auto max-w-4xl">
        <CardHeader>
          <CardTitle>Issue Book</CardTitle>
          <CardDescription>Select a book and a member to issue the book.</CardDescription>
        </CardHeader>
        <form onSubmit={handleSubmit}>
          <CardContent className="space-y-6">
            <Tabs defaultValue="book" className="w-full">
              <TabsList className="grid w-full grid-cols-2">
                <TabsTrigger value="book">Select Book</TabsTrigger>
                <TabsTrigger value="member">Select Member</TabsTrigger>
              </TabsList>
              <TabsContent value="book" className="space-y-4">
                <div className="relative">
                  <Search className="absolute left-2.5 top-2.5 h-4 w-4 text-muted-foreground" />
                  <Input
                    type="search"
                    placeholder="Search by title, author, or ISBN..."
                    className="pl-8 w-full"
                    value={searchQuery}
                    onChange={(e) => setSearchQuery(e.target.value)}
                  />
                </div>

                <div className="h-[300px] overflow-auto border rounded-md">
                  <table className="w-full">
                    <thead className="sticky top-0 bg-background">
                      <tr className="border-b">
                        <th className="text-left p-2">Title</th>
                        <th className="text-left p-2">Author</th>
                        <th className="text-left p-2">ISBN</th>
                        <th className="text-center p-2">Available</th>
                        <th className="text-right p-2">Action</th>
                      </tr>
                    </thead>
                    <tbody>
                      {filteredBooks.map((book) => (
                        <tr key={book.id} className="border-b hover:bg-muted/50">
                          <td className="p-2">{book.title}</td>
                          <td className="p-2">{book.author}</td>
                          <td className="p-2">{book.isbn}</td>
                          <td className="p-2 text-center">{book.available}</td>
                          <td className="p-2 text-right">
                            <Button
                              type="button"
                              variant="outline"
                              size="sm"
                              disabled={book.available === 0}
                              onClick={() => setSelectedBook(book)}
                            >
                              Select
                            </Button>
                          </td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>

                {selectedBook && (
                  <div className="p-4 border rounded-md bg-muted/50">
                    <h3 className="font-medium">Selected Book:</h3>
                    <p>
                      {selectedBook.title} by {selectedBook.author}
                    </p>
                  </div>
                )}
              </TabsContent>

              <TabsContent value="member" className="space-y-4">
                <div className="relative">
                  <Search className="absolute left-2.5 top-2.5 h-4 w-4 text-muted-foreground" />
                  <Input
                    type="search"
                    placeholder="Search by name or ID..."
                    className="pl-8 w-full"
                    value={memberSearchQuery}
                    onChange={(e) => setMemberSearchQuery(e.target.value)}
                  />
                </div>

                <div className="h-[300px] overflow-auto border rounded-md">
                  <table className="w-full">
                    <thead className="sticky top-0 bg-background">
                      <tr className="border-b">
                        <th className="text-left p-2">ID</th>
                        <th className="text-left p-2">Name</th>
                        <th className="text-left p-2">Email</th>
                        <th className="text-center p-2">Books Borrowed</th>
                        <th className="text-right p-2">Action</th>
                      </tr>
                    </thead>
                    <tbody>
                      {filteredMembers.map((member) => (
                        <tr key={member.id} className="border-b hover:bg-muted/50">
                          <td className="p-2">{member.id}</td>
                          <td className="p-2">{member.name}</td>
                          <td className="p-2">{member.email}</td>
                          <td className="p-2 text-center">{member.booksBorrowed}</td>
                          <td className="p-2 text-right">
                            <Button type="button" variant="outline" size="sm" onClick={() => setSelectedMember(member)}>
                              Select
                            </Button>
                          </td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>

                {selectedMember && (
                  <div className="p-4 border rounded-md bg-muted/50">
                    <h3 className="font-medium">Selected Member:</h3>
                    <p>
                      {selectedMember.name} (ID: {selectedMember.id})
                    </p>
                  </div>
                )}
              </TabsContent>
            </Tabs>

            <div className="space-y-2">
              <Label htmlFor="dueDate">Due Date</Label>
              <Input id="dueDate" name="dueDate" type="date" required min={new Date().toISOString().split("T")[0]} />
            </div>
          </CardContent>
          <CardFooter className="flex justify-between">
            <Button variant="outline" type="button" onClick={() => router.push("/transactions")}>
              Cancel
            </Button>
            <Button type="submit" disabled={isSubmitting || !selectedBook || !selectedMember}>
              {isSubmitting ? "Processing..." : "Issue Book"}
            </Button>
          </CardFooter>
        </form>
      </Card>
    </div>
  )
}

interface Book {
  id: number
  title: string
  author: string
  isbn: string
  available: number
}

interface Member {
  id: string
  name: string
  email: string
  booksBorrowed: number
}

const books: Book[] = [
  {
    id: 1,
    title: "To Kill a Mockingbird",
    author: "Harper Lee",
    isbn: "9780061120084",
    available: 3,
  },
  {
    id: 2,
    title: "1984",
    author: "George Orwell",
    isbn: "9780451524935",
    available: 2,
  },
  {
    id: 3,
    title: "The Great Gatsby",
    author: "F. Scott Fitzgerald",
    isbn: "9780743273565",
    available: 1,
  },
  {
    id: 4,
    title: "Sapiens: A Brief History of Humankind",
    author: "Yuval Noah Harari",
    isbn: "9780062316097",
    available: 4,
  },
  {
    id: 5,
    title: "A Brief History of Time",
    author: "Stephen Hawking",
    isbn: "9780553380163",
    available: 0,
  },
]

const members: Member[] = [
  {
    id: "M001",
    name: "John Doe",
    email: "john.doe@example.com",
    booksBorrowed: 2,
  },
  {
    id: "M002",
    name: "Jane Smith",
    email: "jane.smith@example.com",
    booksBorrowed: 1,
  },
  {
    id: "M003",
    name: "Robert Johnson",
    email: "robert.j@example.com",
    booksBorrowed: 0,
  },
  {
    id: "M004",
    name: "Emily Davis",
    email: "emily.davis@example.com",
    booksBorrowed: 3,
  },
  {
    id: "M005",
    name: "Michael Wilson",
    email: "michael.w@example.com",
    booksBorrowed: 0,
  },
]
