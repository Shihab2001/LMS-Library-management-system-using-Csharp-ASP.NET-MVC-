import Link from "next/link"
import { Search, UserPlus } from "lucide-react"

import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import { Badge } from "@/components/ui/badge"

export default function MembersPage() {
  return (
    <div className="container mx-auto py-6">
      <div className="flex items-center justify-between mb-6">
        <h1 className="text-3xl font-bold tracking-tight">Members Management</h1>
        <Link href="/members/register">
          <Button>
            <UserPlus className="mr-2 h-4 w-4" />
            Register Member
          </Button>
        </Link>
      </div>

      <div className="flex flex-col md:flex-row gap-4 mb-6">
        <div className="relative flex-1">
          <Search className="absolute left-2.5 top-2.5 h-4 w-4 text-muted-foreground" />
          <Input type="search" placeholder="Search by name, ID, or contact info..." className="pl-8 w-full" />
        </div>
      </div>

      <div className="rounded-md border">
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Member ID</TableHead>
              <TableHead>Name</TableHead>
              <TableHead>Email</TableHead>
              <TableHead>Phone</TableHead>
              <TableHead>Status</TableHead>
              <TableHead>Books Borrowed</TableHead>
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {members.map((member) => (
              <TableRow key={member.id}>
                <TableCell className="font-medium">{member.id}</TableCell>
                <TableCell>{member.name}</TableCell>
                <TableCell>{member.email}</TableCell>
                <TableCell>{member.phone}</TableCell>
                <TableCell>
                  <Badge variant={member.status === "Active" ? "default" : "secondary"}>{member.status}</Badge>
                </TableCell>
                <TableCell>{member.booksBorrowed}</TableCell>
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

const members = [
  {
    id: "M001",
    name: "John Doe",
    email: "john.doe@example.com",
    phone: "(555) 123-4567",
    status: "Active",
    booksBorrowed: 2,
  },
  {
    id: "M002",
    name: "Jane Smith",
    email: "jane.smith@example.com",
    phone: "(555) 987-6543",
    status: "Active",
    booksBorrowed: 1,
  },
  {
    id: "M003",
    name: "Robert Johnson",
    email: "robert.j@example.com",
    phone: "(555) 456-7890",
    status: "Inactive",
    booksBorrowed: 0,
  },
  {
    id: "M004",
    name: "Emily Davis",
    email: "emily.davis@example.com",
    phone: "(555) 789-0123",
    status: "Active",
    booksBorrowed: 3,
  },
  {
    id: "M005",
    name: "Michael Wilson",
    email: "michael.w@example.com",
    phone: "(555) 234-5678",
    status: "Active",
    booksBorrowed: 0,
  },
  {
    id: "M006",
    name: "Sarah Brown",
    email: "sarah.b@example.com",
    phone: "(555) 876-5432",
    status: "Active",
    booksBorrowed: 1,
  },
  {
    id: "M007",
    name: "David Miller",
    email: "david.m@example.com",
    phone: "(555) 345-6789",
    status: "Inactive",
    booksBorrowed: 0,
  },
]
