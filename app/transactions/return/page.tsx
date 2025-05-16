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
import { Badge } from "@/components/ui/badge"

export default function ReturnBookPage() {
  const router = useRouter()
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [selectedTransaction, setSelectedTransaction] = useState<Transaction | null>(null)
  const [searchQuery, setSearchQuery] = useState("")
  const [lateFee, setLateFee] = useState(0)

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()

    if (!selectedTransaction) {
      toast({
        title: "Error",
        description: "Please select a transaction",
        variant: "destructive",
      })
      return
    }

    setIsSubmitting(true)

    // Simulate API call
    setTimeout(() => {
      toast({
        title: "Book Returned Successfully",
        description: `"${selectedTransaction.bookTitle}" has been returned${lateFee > 0 ? ` with a late fee of $${lateFee}` : ""}.`,
      })
      setIsSubmitting(false)
      router.push("/transactions")
    }, 1000)
  }

  const filteredTransactions = transactions.filter(
    (transaction) =>
      (transaction.bookTitle.toLowerCase().includes(searchQuery.toLowerCase()) ||
        transaction.memberName.toLowerCase().includes(searchQuery.toLowerCase()) ||
        transaction.id.includes(searchQuery)) &&
      transaction.status !== "Returned",
  )

  const calculateLateFee = (dueDate: string) => {
    const today = new Date()
    const due = new Date(dueDate)

    if (today > due) {
      const diffTime = Math.abs(today.getTime() - due.getTime())
      const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24))
      return diffDays * 0.5 // $0.50 per day
    }

    return 0
  }

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
          <CardTitle>Return Book</CardTitle>
          <CardDescription>Select a book to return from the list of issued books.</CardDescription>
        </CardHeader>
        <form onSubmit={handleSubmit}>
          <CardContent className="space-y-6">
            <div className="relative">
              <Search className="absolute left-2.5 top-2.5 h-4 w-4 text-muted-foreground" />
              <Input
                type="search"
                placeholder="Search by book title, member name, or transaction ID..."
                className="pl-8 w-full"
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
              />
            </div>

            <div className="h-[300px] overflow-auto border rounded-md">
              <table className="w-full">
                <thead className="sticky top-0 bg-background">
                  <tr className="border-b">
                    <th className="text-left p-2">ID</th>
                    <th className="text-left p-2">Book Title</th>
                    <th className="text-left p-2">Member</th>
                    <th className="text-left p-2">Issue Date</th>
                    <th className="text-left p-2">Due Date</th>
                    <th className="text-center p-2">Status</th>
                    <th className="text-right p-2">Action</th>
                  </tr>
                </thead>
                <tbody>
                  {filteredTransactions.map((transaction) => (
                    <tr key={transaction.id} className="border-b hover:bg-muted/50">
                      <td className="p-2">{transaction.id}</td>
                      <td className="p-2">{transaction.bookTitle}</td>
                      <td className="p-2">{transaction.memberName}</td>
                      <td className="p-2">{transaction.issueDate}</td>
                      <td className="p-2">{transaction.dueDate}</td>
                      <td className="p-2 text-center">
                        <Badge variant={transaction.status === "Overdue" ? "destructive" : "default"}>
                          {transaction.status}
                        </Badge>
                      </td>
                      <td className="p-2 text-right">
                        <Button
                          type="button"
                          variant="outline"
                          size="sm"
                          onClick={() => {
                            setSelectedTransaction(transaction)
                            setLateFee(calculateLateFee(transaction.dueDate))
                          }}
                        >
                          Select
                        </Button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>

            {selectedTransaction && (
              <div className="p-4 border rounded-md bg-muted/50 space-y-4">
                <div>
                  <h3 className="font-medium">Selected Book:</h3>
                  <p>{selectedTransaction.bookTitle}</p>
                  <p className="text-sm text-muted-foreground">
                    Borrowed by: {selectedTransaction.memberName} on {selectedTransaction.issueDate}
                  </p>
                </div>

                {lateFee > 0 && (
                  <div>
                    <h3 className="font-medium text-destructive">Late Fee:</h3>
                    <p>${lateFee.toFixed(2)}</p>
                    <p className="text-sm text-muted-foreground">Due date was {selectedTransaction.dueDate}</p>
                  </div>
                )}

                <div className="space-y-2">
                  <Label htmlFor="condition">Book Condition</Label>
                  <Input id="condition" name="condition" placeholder="Note any damage or issues with the book" />
                </div>
              </div>
            )}
          </CardContent>
          <CardFooter className="flex justify-between">
            <Button variant="outline" type="button" onClick={() => router.push("/transactions")}>
              Cancel
            </Button>
            <Button type="submit" disabled={isSubmitting || !selectedTransaction}>
              {isSubmitting ? "Processing..." : "Return Book"}
            </Button>
          </CardFooter>
        </form>
      </Card>
    </div>
  )
}

interface Transaction {
  id: string
  bookTitle: string
  memberName: string
  issueDate: string
  dueDate: string
  returnDate: string | null
  status: "Issued" | "Overdue" | "Returned"
  fine: number | null
}

const transactions: Transaction[] = [
  {
    id: "T002",
    bookTitle: "1984",
    memberName: "Jane Smith",
    issueDate: "2023-04-18",
    dueDate: "2023-05-02",
    returnDate: null,
    status: "Issued",
    fine: null,
  },
  {
    id: "T003",
    bookTitle: "The Great Gatsby",
    memberName: "Emily Davis",
    issueDate: "2023-04-10",
    dueDate: "2023-04-24",
    returnDate: null,
    status: "Overdue",
    fine: 3.5,
  },
  {
    id: "T004",
    bookTitle: "Sapiens: A Brief History of Humankind",
    memberName: "Michael Wilson",
    issueDate: "2023-04-20",
    dueDate: "2023-05-04",
    returnDate: null,
    status: "Issued",
    fine: null,
  },
  {
    id: "T006",
    bookTitle: "Educated",
    memberName: "John Doe",
    issueDate: "2023-04-22",
    dueDate: "2023-05-06",
    returnDate: null,
    status: "Issued",
    fine: null,
  },
]
