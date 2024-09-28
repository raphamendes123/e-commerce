FROM mcr.microsoft.com/mssql/rhel/server:2019-CU1-rhel-8 AS base 
RUN mkdir -p /usr/work
WORKDIR /usr/work
COPY . /usr/work/

#executar o comando entrypoint EXPOSE 1433
CMD /bin/bash ./entrypoint.sh




 