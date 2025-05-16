import Link from "next/link"
import { BookCheck, BookX, Search } from "lucide-react"

import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import { Badge } from "@/components/ui/badge"

export default function TransactionsPage() {
  return (
    <div className="container mx-auto py-6">
      <div className="flex items-center justify-between mb-6">
        <h1 className="text-3xl font-bold tracking-tight">Transactions</h1>
        <div className="flex gap-2">
          <Link href="/transactions/issue">
            <Button>
              <BookCheck className="mr-2 h-4 w-4" />
              Issue Book
            </Button>
          </Link>
          <Link href="/transactions/return">
            <Button variant="outline">
              <BookX className="mr-2 h-4 w-4" />
              Return Book
            </Button>
          </Link>
        </div>
      </div>

      <div className="flex flex-col md:flex-row gap-4 mb-6">
        <div className="relative flex-1">
          <Search className="absolute left-2.5 top-2.5 h-4 w-4 text-muted-foreground" />
          <Input type="search" placeholder="Search by book title, member name, or ID..." className="pl-8 w-full" />
        </div>
        <div className="flex gap-2">
          <Select defaultValue="all">
            <SelectTrigger className="w-[180px]">
              <SelectValue placeholder="Status" />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="all">All Transactions</SelectItem>
              <SelectItem value="issued">Issued</SelectItem>
              <SelectItem value="returned">Returned</SelectItem>
              <SelectItem value="overdue">Overdue</SelectItem>
            </SelectContent>
          </Select>
        </div>
      </div>

      <div className="rounded-md border">
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Transaction ID</TableHead>
              <TableHead>Book Title</TableHead>
              <TableHead>Member</TableHead>
              <TableHead>Issue Date</TableHead>
              <TableHead>Due Date</TableHead>
              <TableHead>Return Date</TableHead>
              <TableHead>Status</TableHead>
              <TableHead>Fine</TableHead>
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {transactions.map((transaction) => (
              <TableRow key={transaction.id}>
                <TableCell className="font-medium">{transaction.id}</TableCell>
                <TableCell>{transaction.bookTitle}</TableCell>
                <TableCell>{transaction.memberName}</TableCell>
                <TableCell>{transaction.issueDate}</TableCell>
                <TableCell>{transaction.dueDate}</TableCell>
                <TableCell>{transaction.returnDate || "-"}</TableCell>
                <TableCell>
                  <Badge
                    variant={
                      transaction.status === "Overdue"
                        ? "destructive"
                        : transaction.status === "Issued"
                          ? "default"
                          : "secondary"
                    }
                  >
                    {transaction.status}
                  </Badge>
                </TableCell>
                <TableCell>{transaction.fine ? `$${transaction.fine}` : "-"}</TableCell>
                <TableCell className="text-right">
                  <Button variant="outline" size="sm">
                    Details
                  </Button>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </div>
    </div>
  )
}

const transactions = [
  {
    id: "T001",
    bookTitle: "To Kill a Mockingbird",
    memberName: "John Doe",
    issueDate: "2023-04-15",
    dueDate: "2023-04-29",
    returnDate: "2023-04-28",
    status: "Returned",
    fine: null,
  },
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
    id: "T005",
    bookTitle: "A Brief History of Time",
    memberName: "Sarah Brown",
    issueDate: "2023-04-05",
    dueDate: "2023-04-19",
    returnDate: "2023-04-22",
    status: "Returned",
    fine: 1.5,
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
