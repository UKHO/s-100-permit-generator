variable "name" {
  type = string
}

variable "resource_group_name" {
  type = string
}

variable "location" {
  type = string
}

variable "tenant_id" {
  type = string
}

variable "env_name" {
  type = string
}

variable "read_access_objects" {
  type = map(string)
}

variable "secrets" {
  type = map(string)
}

variable "secrets_kv2" {
  type = map(string)
}
variable "tags" {

}
variable "name_kv2" {

}

