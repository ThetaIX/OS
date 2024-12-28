obj-m += tsu_module.o
#KERNELDIR := $(shell find /nix/store -name build | grep linux-$(shell uname -r) | head -n 1)
KERNELDIR := /lib/modules/$(shell uname -r)/build

all:
	make -C $(KERNELDIR) M=$(PWD) modules

clean:
	make -C $(KERNELDIR) M=$(PWD) clean

