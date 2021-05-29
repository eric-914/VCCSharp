typedef struct {
  unsigned char* MemPages[1024];
} TC1014State;

static TC1014State* instance = new TC1014State();

extern "C" {
  __declspec(dllexport) TC1014State* __cdecl GetTC1014State() {
    return instance;
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl MemRead8(unsigned short mmu, unsigned short mask)
  {
    return(instance->MemPages[mmu][mask]);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl MemWrite8(unsigned char data, unsigned short mmu, unsigned short mask)
  {
    instance->MemPages[mmu][mask] = data;
  }
}
