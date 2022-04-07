terraform {
  required_providers {
    azurerm = {
      source = "hashicorp/azurerm"
      version = ">=3.0.0"
    }
  }
}

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "BaseGroup" {
    name = "TerraformBaseGroup"
    location = "West Europe"
}

resource "azurerm_container_group" "BaseContainer" {
  name = "TerraformBaseContainerInstance"
  location = azurerm_resource_group.BaseGroup.location
  resource_group_name = azurerm_resource_group.BaseGroup.name
  ip_address_type = "Public"
  dns_name_label = "ir0nsideeTFBase"
  os_type = "Linux"

  container {
    name = "terraformbase"
    image = "ir0nsidee/terraformbase:latest"
    cpu = 1
    memory = 1
    ports {
      port = 80
      protocol = "TCP"
    }
  }
}