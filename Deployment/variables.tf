variable "location" {
  type    = string
  default = "uksouth"
}

variable "resource_group_name" {
  type    = string
  default = "ps"
}

locals {
  env_name           = lower(terraform.workspace)
  service_name       = "ps"  
  web_app_name       = "${local.service_name}-${local.env_name}-api"
  stub_web_app_name  = "${local.service_name}-${local.env_name}-stub"
  key_vault_name     = "${local.service_name}-ukho-${local.env_name}-kv"
  key_vault_midkv    = "${local.service_name}-ukho-${local.env_name}-midkv"
  ###
  storage_name       = "${local.service_name}${local.env_name}storage"
  container_name     = "erp-container"
  pe_identity = "pe${local.env_name}pe"
 
  vnet_link = "pe${local.env_name}pe"
  private_connection = "/subscriptions/${var.subscription_id}/resourceGroups/pe-${local.env_name}-rg/providers/Microsoft.Web/sites/pe-${local.env_name}-api"
  dns_resource_group = "engineering-rg"
  zone_group = "pe${local.env_name}2pezone"
  dns_zones = "privatelink.azurewebsites.net"
  tags = {
    SERVICE                   = "S100 Permit Service"
    ENVIRONMENT               = local.env_name
    SERVICE_OWNER             = "UKHO"
    RESPONSIBLE_TEAM          = "Mastek"
    CALLOUT_TEAM              = "On-Call_N/A"
    COST_CENTRE               = "A.011.15.5"
    }
  }

variable "sku_name" {
  type = map(any)
  default = {
            "dev"       =  "P1v2"            
            "vni"       =  "P1v3"
            "vne"       =  "P1v3"
            "iat"       =  "P1v3"
            }
}

variable "spoke_rg" {
  type = string
}

variable "spoke_vnet_name" {
  type = string
}

variable "spoke_subnet_name" {
  type = string
}