#基础镜像（用来构建镜像）
FROM microsoft/dotnet:2.1-sdk
WORKDIR /app
COPY . .
ENV TZ=Asia/Shanghai
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
EXPOSE 80
ENTRYPOINT ["dotnet", "Boilerplate.WebHost.dll"]