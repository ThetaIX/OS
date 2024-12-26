obj-m += tsu_module.o
KERNEL_BUILD := $(shell find /nix/store -name build | grep linux-$(shell uname -r) | head -n 1)
# KERNELDIR := /lib/modules/$(shell uname -r)/build

all:
	make -C $(KERNEL_BUILD) M=$(PWD) modules

clean:
	make -C $(KERNEL_BUILD) M=$(PWD) clean

